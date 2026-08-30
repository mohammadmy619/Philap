using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;
using Domain.TripAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Consumers
{
    public class UpdateTripEventConsumer : IConsumer<IUpdateTripEvent>
    {
        private readonly ILogger<UpdateTripEventConsumer> _logger;
        private readonly ITripRepository _tripRepository;

        public UpdateTripEventConsumer(
            ILogger<UpdateTripEventConsumer> logger,
            ITripRepository tripRepository)
        {
            _logger = logger;
            _tripRepository = tripRepository;
        }

        public async Task Consume(ConsumeContext<IUpdateTripEvent> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation(
                    "Processing UpdateTripEvent | TicketId: {TicketId}, Current TripId: {TripId}, New TripId: {NewTripId}",
                    message.TicketId,
                    message.TripId,
                    message.NewTripId);

                // تعیین شناسه سفر مقصد (اگر سفر جدیدی مشخص شده بود همان را می‌گیریم، وگرنه سفر فعلی)
                var targetTripId = message.NewTripId.HasValue && message.NewTripId.Value != Guid.Empty
                    ? message.NewTripId.Value
                    : message.TripId;

                var targetTrip = await _tripRepository.GetTripByIdAsync(targetTripId, context.CancellationToken);

                if (targetTrip is null)
                {
                    _logger.LogWarning("Target trip not found | TripId: {TripId}", targetTripId);

                    // ارسال پیام شکست/لغو در آپدیت سفر
                    await context.Publish<ICancelUpdateTripEvent>(new
                    {
                        CorrelationId = message.CorrelationId,
                        TicketId = message.TicketId,
                        TripId = message.TripId,
                        PassengerId = message.PassengerId,
                        Reason = $"Trip with ID {targetTripId} was not found.",
                        CancelledDate = DateTime.UtcNow
                    }, context.CancellationToken);

                    return;
                }

                // اگر سفر تغییر کرده باشد، تیکت را از سفر قبلی جدا کرده و به سفر جدید اختصاص می‌دهیم
                if (message.NewTripId.HasValue && message.NewTripId.Value != message.TripId)
                {
                    var oldTrip = await _tripRepository.GetTripByIdAsync(message.TripId, context.CancellationToken);
                    if (oldTrip is not null)
                    {
                        oldTrip.RemoveTicketId(message.TicketId);
                    }

                    targetTrip.AddTicketId(message.TicketId);
                }

                await _tripRepository.SaveChangesAsync(context.CancellationToken);

                // انتشار رویداد موفقیت مرحله اول و ارسال به مرحله بعد (آپدیت مسافر)
                await context.Publish<IUpdatePersonEvent>(new
                {
                    CorrelationId = message.CorrelationId,
                    TicketId = message.TicketId,
                    TripId = targetTrip.Id,
                    PassengerId = message.PassengerId,
                    LeaderId = targetTrip.LeaderId,
                    Status = "TripUpdated",
                    TicketUpdatedDate = DateTime.UtcNow
                }, context.CancellationToken);

                _logger.LogInformation(
                    "UpdateTripEvent processed successfully and IUpdatePersonEvent published | TicketId: {TicketId}, TripId: {TripId}",
                    message.TicketId,
                    targetTrip.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "UpdateTripEvent processing failed | TicketId: {TicketId}, TripId: {TripId}, Error: {ErrorMessage}",
                    message.TicketId,
                    message.TripId,
                    ex.Message);

                // در صورت بروز خطای سیستمی، رویداد شکست منتشر می‌شود
                await context.Publish<ICancelUpdateTripEvent>(new
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
