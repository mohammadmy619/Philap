using BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents;
using Domain.Persons.Leader;
using Domain.Persons.Passenger;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Consumers.CancellationTicketEvents
{
    public class PersonCancellationEventConsumer : IConsumer<IPersonCancellationEvent>
    {
        private readonly ILogger<PersonCancellationEventConsumer> _logger;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ILeaderRepository _leaderRepository;

        public PersonCancellationEventConsumer(
            ILogger<PersonCancellationEventConsumer> logger,
            IPassengerRepository passengerRepository,
            ILeaderRepository leaderRepository)
        {
            _logger = logger;
            _passengerRepository = passengerRepository;
            _leaderRepository = leaderRepository;
        }

        public async Task Consume(ConsumeContext<IPersonCancellationEvent> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation(
                    "Starting person cancellation process | TicketId: {TicketId}, TripId: {TripId}, PassengerId: {PassengerId}",
                    message.TicketId,
                    message.TripId,
                    message.PassengerId);

                bool passengerUpdated = false;
                bool leaderUpdated = false;

                // ۱. لغو عملیات روی Passenger
                var passenger = await _passengerRepository.GetPassengerByIdAsync(
                    message.PassengerId,
                    context.CancellationToken);

                if (passenger is not null)
                {
                    _logger.LogInformation("Removing ticket and trip from passenger | PassengerId: {PassengerId}", passenger.Id);

                    passenger.RemoveTicketId(message.TicketId);
                    passenger.RemoveTripId(message.TripId);

                    await _passengerRepository.SaveChangesAsync(context.CancellationToken);
                    passengerUpdated = true;
                }
                else
                {
                    _logger.LogWarning("Passenger not found (might be already cancelled) | PassengerId: {PassengerId}", message.PassengerId);
                }

              
                var leader = await _leaderRepository.GetLeaderByTripIdAsync(
                    message.TripId,
                    context.CancellationToken);

                if (leader is not null)
                {
                    _logger.LogInformation("Removing trip from leader | LeaderId: {LeaderId}", leader.Id);

                    leader.RemoveTripId(message.TripId);

                    await _leaderRepository.SaveChangesAsync(context.CancellationToken);
                    leaderUpdated = true;
                }
                else
                {
                    _logger.LogWarning("Leader not found for this trip (might be already cancelled) | TripId: {TripId}", message.TripId);
                  
                    await context.Publish<IPersonCancellationFailedEvent>(new
                    {
                        CorrelationId = message.CorrelationId,
                        TicketId = message.TicketId,
                        TripId = message.TripId,
                        PassengerId = message.PassengerId,
                        CurrentState = "PersonCancellationSkipped", 
                        TicketCreatedDate = message.TicketCreatedDate,
                        TicketCancelDate = DateTime.UtcNow, 
                        Status = "PersonCancellationSkipped",
                        CompletedAt = DateTime.UtcNow, 
                        Reason = "Leader not found, assuming already cancelled or not applicable."
                    }, context.CancellationToken);
                }
            

 
                await context.Publish<ICancellationConfirmedEvent>(new
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
                    TicketCancelDate = message.TicketCancelDate,
                    Status = "Cancelled",

                    CancelledDate = message.CancelledAt,
                    Reason = "Person and Leader cancellation processed successfully (or already cancelled)"
                }, context.CancellationToken);

                _logger.LogInformation(
                    "Person cancellation completed and confirmed event published | TicketId: {TicketId}",
                    message.TicketId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Person cancellation compensation failed | TicketId: {TicketId}, Error: {ErrorMessage}",
                    message.TicketId,
                    ex.Message);

                await context.Publish<IPersonCancellationFailedEvent>(new
                {
                    CorrelationId = message.CorrelationId,
                    TicketId = message.TicketId,
                    TripId = message.TripId,
                    PassengerId = message.PassengerId,
                    CurrentState = "PersonCancellationFailed",
                    TicketCreatedDate = message.TicketCreatedDate,
                    TicketCancelDate = message.TicketCreatedDate,
                    Status = "PersonCancellationFailed",
                    FailedAt = DateTime.UtcNow,
                    Reason = $"Exception occurred during person cancellation: {ex.Message}"
                }, context.CancellationToken);

                throw; 
            }
        }
    }
}