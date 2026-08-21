using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.Implements;
using Domain.Persons.Leader;
using Domain.Persons.Passenger;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Consumers
{


    //کنسل کلی 
    //جدا
    public class CancelTicketEventConsumer : IConsumer<ICancelTicketEvent>
    {
        private readonly ILogger<CancelTicketEventConsumer> _logger;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ILeaderRepository _leaderRepository;

        public CancelTicketEventConsumer(
            ILogger<CancelTicketEventConsumer> logger,
            IPassengerRepository passengerRepository,
            ILeaderRepository leaderRepository)
        {
            _logger = logger;
            _passengerRepository = passengerRepository;
            _leaderRepository = leaderRepository;
        }

        public async Task Consume(ConsumeContext<ICancelTicketEvent> context)
        {
            try
            {
                _logger.LogInformation(
                    "Starting person cancellation compensation | TicketId: {TicketId}, TripId: {TripId}",
                    context.Message.TicketId,
                    context.Message.TripId);

                var passenger = await _passengerRepository.GetPassengerByIdAsync(
                    context.Message.PassengerId,
                    context.CancellationToken);

                var leader = await _leaderRepository.GetLeaderByTripIdAsync(
                    context.Message.TripId, // فرض بر این است که LeaderId در پیام لغو هم وجود دارد
                    context.CancellationToken);

                // 1. لغو عملیات روی Passenger
                if (passenger is not null)
                {
                    _logger.LogInformation(
                        "Removing ticket and trip from passenger | PassengerId: {PassengerId}",
                        passenger.Id);

                    // ⚠️ نکته: مطمئن شوید این متدها در Aggregate مسافر تعریف شده‌اند
                    passenger.RemoveTicketId(context.Message.TicketId);
                    passenger.RemoveTripId(context.Message.TripId);

                    await _passengerRepository.SaveChangesAsync(context.CancellationToken);
                }
                else
                {
                    _logger.LogWarning(
                        "Passenger not found for cancellation. Skipping passenger update | PassengerId: {PassengerId}",
                        context.Message.PassengerId);
                }

                // 2. لغو عملیات روی Leader
                if (leader is not null)
                {
                    _logger.LogInformation(
                        "Removing trip from leader | LeaderId: {LeaderId}",
                        leader.Id);

                
                    leader.RemoveTripId(context.Message.TripId);

                    await _leaderRepository.SaveChangesAsync(context.CancellationToken);
                }
                else
                {
                    _logger.LogWarning(
                        "Leader not found for cancellation. Skipping leader update | TripId: {TripId}",
                        context.Message.TripId);
                }

                _logger.LogInformation(
                    "Person cancellation compensation completed successfully | TicketId: {TicketId}",
                    context.Message.TicketId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Person cancellation compensation failed | TicketId: {TicketId}, Error: {ErrorMessage}",
                    context.Message.TicketId,
                    ex.Message);

                throw; 
            }
        }
    }
}