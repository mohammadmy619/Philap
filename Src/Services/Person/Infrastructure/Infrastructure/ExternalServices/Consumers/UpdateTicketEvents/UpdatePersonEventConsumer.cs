using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;
using Domain.Persons.Leader;
using Domain.Persons.Passenger;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Consumers.UpdateTicketEvents
{
    public class UpdatePersonEventConsumer : IConsumer<IUpdatePersonEvent>
    {
        private readonly ILogger<UpdatePersonEventConsumer> _logger;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ILeaderRepository _leaderRepository;

        public UpdatePersonEventConsumer(
            ILogger<UpdatePersonEventConsumer> logger,
            IPassengerRepository passengerRepository,
            ILeaderRepository leaderRepository)
        {
            _logger = logger;
            _passengerRepository = passengerRepository;
            _leaderRepository = leaderRepository;
        }

        public async Task Consume(ConsumeContext<IUpdatePersonEvent> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation(
                    "Processing UpdatePersonEvent | TicketId: {TicketId}, PassengerId: {PassengerId}, NewPassengerId: {NewPassengerId}, LeaderId: {LeaderId}",
                    message.TicketId,
                    message.PassengerId,
                    message.NewPassengerId,
                    message.LeaderId);

                // تعیین مسافر هدف (اگر مسافر تغییر کرده بود شناسه جدید، در غیر این صورت شناسه فعلی)
                var targetPassengerId = message.NewPassengerId.HasValue && message.NewPassengerId.Value != Guid.Empty
                    ? message.NewPassengerId.Value
                    : message.PassengerId;

                var passenger = await _passengerRepository.GetPassengerByIdAsync(
                    targetPassengerId,
                    context.CancellationToken);

                // بررسی وجود لیدر در صورت ارسال LeaderId
                var leader = message.LeaderId.HasValue && message.LeaderId.Value != Guid.Empty
                    ? await _leaderRepository.GetLeaderByIdAsync(message.LeaderId.Value, context.CancellationToken)
                    : null;

                // اعتبارسنجی موجودیت‌ها
                if (passenger is not null && (message.LeaderId == null || leader is not null))
                {
                    // اگر مسافر تغییر کرده باشد، تیکت از مسافر قبلی جدا شده و به مسافر جدید اختصاص داده می‌شود
                    if (message.NewPassengerId.HasValue && message.NewPassengerId.Value != message.PassengerId)
                    {
                        var oldPassenger = await _passengerRepository.GetPassengerByIdAsync(
                            message.PassengerId,
                            context.CancellationToken);

                        if (oldPassenger is not null)
                        {
                            oldPassenger.RemoveTicketId(message.TicketId);
                        }

                        passenger.AddTicketId(message.TicketId);
                    }

                    // به‌روزرسانی سفر برای مسافر
                    passenger.AddTripId(message.TripId);
                    await _passengerRepository.SaveChangesAsync(context.CancellationToken);

                    // به‌روزرسانی سفر برای لیدر در صورت وجود
                    if (leader is not null)
                    {
                        leader.AddTripId(message.TripId);
                        await _leaderRepository.SaveChangesAsync(context.CancellationToken);
                    }

                    // انتشار رویداد تایید و نهایی‌سازی به روزرسانی تیکت
                    await context.Publish<IAcceptUpdateTicketEvent>(new
                    {
                        CorrelationId = message.CorrelationId,
                        TicketId = message.TicketId,
                        TripId = message.TripId,
                        PassengerId = passenger.Id,
                        PriceAmount = 0m, // در صورت نیاز به محاسبه مابه‌التفاوت قیمت قرار دهید
                        PriceCurrency = "IRR",
                        Status = "UpdateAccepted",
                        CompletedDate = DateTime.UtcNow
                    }, context.CancellationToken);

                    _logger.LogInformation(
                        "UpdatePersonEvent processed successfully and IAcceptUpdateTicketEvent published | TicketId: {TicketId}",
                        message.TicketId);
                }
                else
                {
                    _logger.LogWarning(
                        "ICancelUpdatePersonEvent published because Passenger or Leader was not found | TicketId: {TicketId}, PassengerFound: {PassengerFound}, LeaderFound: {LeaderFound}",
                        message.TicketId,
                        passenger is not null,
                        leader is not null);

                    // ارسال پیام شکست در آپدیت مسافر/لیدر
                    await context.Publish<ICancelUpdatePersonEvent>(new
                    {
                        CorrelationId = message.CorrelationId,
                        TicketId = message.TicketId,
                        TripId = message.TripId,
                        PassengerId = message.PassengerId,
                        Reason = "Passenger or Leader entity not found.",
                        CancelledDate = DateTime.UtcNow
                    }, context.CancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "UpdatePersonEvent processing failed | TicketId: {TicketId}, Error: {ErrorMessage}",
                    message.TicketId,
                    ex.Message);

                await context.Publish<ICancelUpdatePersonEvent>(new
                {
                    CorrelationId = message.CorrelationId,
                    TicketId = message.TicketId,
                    TripId = message.TripId,
                    PassengerId = message.PassengerId,
                    Reason = ex.Message,
                    CancelledDate = DateTime.UtcNow
                }, context.CancellationToken);

                throw;
            }
        }
    }
}
