using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;
using Domain.TripAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Consumers.UpdateTicketEvents
{
 
    public class CancelUpdateTripEventConsumer : IConsumer<ICancelUpdatePersonEvent>
    {
        private readonly ILogger<CancelUpdateTripEventConsumer> _logger;
        private readonly ITripRepository _tripRepository;

        public CancelUpdateTripEventConsumer(
            ILogger<CancelUpdateTripEventConsumer> logger,
            ITripRepository tripRepository)
        {
            _logger = logger;
            _tripRepository = tripRepository;
        }

        public async Task Consume(ConsumeContext<ICancelUpdatePersonEvent> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogWarning(
                    "Compensating Trip Update due to Person update failure | TicketId: {TicketId}, TripId: {TripId}",
                    message.TicketId,
                    message.TripId);

                var trip = await _tripRepository.GetTripByIdAsync(
                    message.TripId,
                    context.CancellationToken);

                if (trip is not null)
                {
                    // در صورت نیاز به Rollback: برگرداندن تغییرات سفر
                    trip.RemoveTicketId(message.TicketId);

                    await _tripRepository.SaveChangesAsync(context.CancellationToken);

                    _logger.LogInformation(
                        "Trip update changes rolled back successfully | TicketId: {TicketId}, TripId: {TripId}",
                        message.TicketId,
                        trip.Id);
                }

                // انتشار رویداد جبران نهایی سفر برای اطلاع State Machine
                await context.Publish<ICancelUpdateTripEvent>(new
                {
                    CorrelationId = message.CorrelationId,
                    TicketId = message.TicketId,
                    TripId = message.TripId,
                    PassengerId = message.PassengerId,
                    Reason = message.Reason ?? "Compensating trip due to passenger update cancellation.",
                    CancelledDate = DateTime.UtcNow
                }, context.CancellationToken);

                _logger.LogInformation(
                    "Published ICancelUpdateTripEvent | TicketId: {TicketId}, TripId: {TripId}",
                    message.TicketId,
                    message.TripId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "CancelUpdateTripEvent processing failed | TicketId: {TicketId}, TripId: {TripId}, Error: {ErrorMessage}",
                    message.TicketId,
                    message.TripId,
                    ex.Message);

                throw; // MassTransit خطا را دریافت کرده و طبق تنظیمات Retry اعمال می‌کند
            }
        }
    }
}
