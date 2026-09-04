using BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents;
using Domain.TripAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Consumers.CancellationTicketEvents
{
    public class TripCancellationFailedEventConsumer : IConsumer<IPersonCancellationFailedEvent>
    {
        private readonly ILogger<TripCancellationFailedEventConsumer> _logger;
        private readonly ITripRepository _tripRepository;

        public TripCancellationFailedEventConsumer(
            ILogger<TripCancellationFailedEventConsumer> logger,
            ITripRepository tripRepository)
        {
            _logger = logger;
            _tripRepository = tripRepository;
        }

        public async Task Consume(ConsumeContext<IPersonCancellationFailedEvent> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation(
                    "Processing person cancellation failure event | TicketId: {TicketId}, TripId: {TripId}, CorrelationId: {CorrelationId}",
                    message.TicketId,
                    message.TripId,
                    message.CorrelationId);

                var trip = await _tripRepository.GetTripByIdAsync(
                    message.TripId,
                    context.CancellationToken);

                if (trip is not null)
                {
                    _logger.LogInformation(
                        "Reverting ticket cancellation for trip due to person service failure | TicketId: {TicketId}, TripId: {TripId}",
                        message.TicketId,
                        trip.Id);

                    // فراخوانی متد دامین برای بازگرداندن وضعیت بلیط به حالت قبل (لغوِ عملیات لغو)
                    trip.RevertTicketCancellation(message.TicketId);

                    await _tripRepository.SaveChangesAsync(context.CancellationToken);

                    _logger.LogInformation(
                        "Trip cancellation reverted successfully in database | TicketId: {TicketId}, TripId: {TripId}",
                        message.TicketId,
                        trip.Id);

                    await context.Publish<ITripCancellationFailedEvent>(new
                    {
                        CorrelationId = message.CorrelationId,
                        TicketId = message.TicketId,
                        TripId = message.TripId,
                        PassengerId = message.PassengerId,
                        CurrentState = "CancellationFailed",
                        TicketCreatedDate = message.TicketCreatedDate,
                        TicketCancelDate = message.TicketCancelDate,
                        Status = "CancellationFailed",
                        FailedAt = DateTime.UtcNow,
                        Reason = "Person cancellation failed. Trip cancellation was reverted successfully to maintain consistency."
                    }, context.CancellationToken);

                    _logger.LogInformation(
                        "Published ITripCancellationFailedEvent (Reverted successfully) | TicketId: {TicketId}, TripId: {TripId}",
                        message.TicketId,
                        message.TripId);
                }
                else
                {
                    _logger.LogWarning(
                        "Trip not found for reverting cancellation | TicketId: {TicketId}, TripId: {TripId}",
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
                        TicketCancelDate = message.TicketCancelDate,
                        Status = "CancellationFailed",
                        FailedAt = DateTime.UtcNow,
                        Reason = $"Trip with Id {message.TripId} not found in Trip Service during revert operation"
                    }, context.CancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "TripCancellationFailedEvent processing failed | TicketId: {TicketId}, TripId: {TripId}, Error: {ErrorMessage}",
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
                    TicketCancelDate = message.TicketCancelDate,
                    Status = "CancellationFailed",
                    FailedAt = DateTime.UtcNow,
                    Reason = $"Exception occurred during revert: {ex.Message}"
                }, context.CancellationToken);

                _logger.LogWarning(
                    "Published ITripCancellationFailedEvent (Exception) | TicketId: {TicketId}, TripId: {TripId}",
                    message.TicketId,
                    message.TripId);

                throw;
            }
        }
    }
}