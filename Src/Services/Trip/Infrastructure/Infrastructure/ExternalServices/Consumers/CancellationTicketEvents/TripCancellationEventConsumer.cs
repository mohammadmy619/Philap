using BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents;
using Domain.TripAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Consumers.CancellationTicketEvents
{
    public class TripCancellationEventConsumer : IConsumer<ITripCancellationEvent>
    {
        private readonly ILogger<TripCancellationEventConsumer> _logger;
        private readonly ITripRepository _tripRepository;

        public TripCancellationEventConsumer(
            ILogger<TripCancellationEventConsumer> logger,
            ITripRepository tripRepository)
        {
            _logger = logger;
            _tripRepository = tripRepository;
        }

        public async Task Consume(ConsumeContext<ITripCancellationEvent> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation(
                    "Processing trip cancellation request | TicketId: {TicketId}, TripId: {TripId}",
                    message.TicketId,
                    message.TripId);

                var trip = await _tripRepository.GetTripByIdAsync(
                    message.TripId,
                    context.CancellationToken);

                if (trip is not null)
                {
                    _logger.LogInformation(
                        "Canceling ticket for trip | TicketId: {TicketId}, TripId: {TripId}",
                        message.TicketId,
                        trip.Id);

                    // فراخوانی متد دامین برای حذف بلیط از سفر
                    trip.RemoveTicketId(message.TicketId);

                    await _tripRepository.SaveChangesAsync(context.CancellationToken);

                    _logger.LogInformation(
                        "Trip cancellation processed successfully | TicketId: {TicketId}, TripId: {TripId}",
                        message.TicketId,
                        trip.Id);

                    await context.Publish<IPersonCancellationEvent>(new
                    {
                        // IIntegrationEvent Properties
                        Id = Guid.NewGuid(),
                        OccurredOn = DateTime.UtcNow,

                        // IBaseTicketEvent Properties
                        CorrelationId = message.CorrelationId,
                        TicketId = message.TicketId,
                        TripId = message.TripId,
                        PassengerId = message.PassengerId,
                        CurrentState = "Cancelled",
                        TicketCreatedDate = message.TicketCreatedDate,
                        TicketCancelDate = message.CancelledAt,
                        Status = "Cancelled",

                        // ITripCancellationConfirmedEvent Properties
                        CancelledDate = message.CancelledAt,
                        Reason = "Trip cancellation successfully processed by Trip Service"
                    }, context.CancellationToken);

                    _logger.LogInformation(
                        "Published Trip Cancellation Completion Event | TicketId: {TicketId}, TripId: {TripId}",
                        message.TicketId,
                        message.TripId);
                }
                else
                {


                    _logger.LogWarning(
                        "Trip not found for cancellation (might be already cancelled) | TicketId: {TicketId}, TripId: {TripId}",
                        message.TicketId,
                        message.TripId);


                    await context.Publish<ITripCancellationFailedEvent>(new
                    {
                        CorrelationId = message.CorrelationId,
                        TicketId = message.TicketId,
                        TripId = message.TripId,
                        PassengerId = message.PassengerId,
                        CurrentState = "CancellationFailed",
                        TicketCreatedDate = message.TicketCreatedDate,
                        TicketCancelDate = message.CancelledAt,
                        Status = "CancellationFailed",
                        FailedAt = DateTime.UtcNow,
                        Reason = $"Trip with Id {message.TripId} not found in Trip Service"
                    }, context.CancellationToken);

                    _logger.LogWarning(
                        "Published ITripCancellationFailedEvent (Trip not found) | TicketId: {TicketId}, TripId: {TripId}",
                        message.TicketId,
                        message.TripId);
                }

                
  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "TripCancellationEvent processing failed | TicketId: {TicketId}, TripId: {TripId}, Error: {ErrorMessage}",
                    message.TicketId,
                    message.TripId,
                    ex.Message);

                await context.Publish<ITripCancellationFailedEvent>(new
                {
                    CorrelationId = message.CorrelationId,
                    TicketId = message.TicketId,
                    TripId = message.TripId,
                    PassengerId = message.PassengerId,
                    CurrentState = "CancellationFailed",
                    TicketCreatedDate = message.TicketCreatedDate,
                    TicketCancelDate = message.CancelledAt,
                    Status = "CancellationFailed",
                    FailedAt = DateTime.UtcNow,
                    Reason = $"Exception occurred: {ex.Message}"
                }, context.CancellationToken);

                _logger.LogWarning(
                    "Published ITripCancellationFailedEvent (Exception) | TicketId: {TicketId}, TripId: {TripId}",
                    message.TicketId,
                    message.TripId);

                throw; // MassTransit عملیات Retry را طبق تنظیمات مدیریت می‌کند
            }
        }
    }
}