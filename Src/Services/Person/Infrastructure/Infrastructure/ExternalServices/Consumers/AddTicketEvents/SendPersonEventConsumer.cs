using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.Implements;
using Domain.Persons.Leader;
using Domain.Persons.Passenger;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Consumers.AddTicketEvents
{
    public class SendPersonEventConsumer : IConsumer<ISendPersonEvent>
    {
        private readonly ILogger<SendPersonEventConsumer> _logger;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ILeaderRepository _leaderRepository;

        public SendPersonEventConsumer(
            ILogger<SendPersonEventConsumer> logger,
            IPassengerRepository passengerRepository,
            ILeaderRepository leaderRepository)
        {
            _logger = logger;
            _passengerRepository = passengerRepository;
            _leaderRepository = leaderRepository;
        }

        public async Task Consume(ConsumeContext<ISendPersonEvent> context)
        {
            try
            {
                var passenger = await _passengerRepository.GetPassengerByIdAsync(
                    context.Message.PassengerId,
                    context.CancellationToken);

                var leader = await _leaderRepository.GetLeaderByIdAsync(
                    context.Message.LeaderId,
                    context.CancellationToken);

                if (passenger is not null && leader is not null)
                {
                    _logger.LogInformation(
                        "Adding ticket to person | TicketId: {TicketId}, PassengerId: {PassengerId}, LeaderId: {LeaderId}",
                        context.Message.TicketId,
                        passenger.Id,
                        leader.Id);

                    passenger.AddTicketId(context.Message.TicketId);
                    passenger.AddTripId(context.Message.TripId);


                    // ذخیره تغییرات هر دو موجودیت
                    await _passengerRepository.SaveChangesAsync(context.CancellationToken);
                    leader.AddTripId(context.Message.TripId);

                    await _leaderRepository.SaveChangesAsync(context.CancellationToken);

                    await context.Publish<IAcceptTicketEvent>(new
                    {
                        // پراپرتی‌های پایه (IBaseTicketEvent)
                        CorrelationId = context.Message.CorrelationId,
                        TicketId = context.Message.TicketId,
                        TripId = context.Message.TripId,
                        PassengerId = context.Message.PassengerId,
                        TicketCreatedDate = context.Message.TicketCreatedDate,
                        TicketCancelDate = context.Message.TicketCancelDate,

                        CurrentState = "TicketAccepted",
                        Status = "Accepted",

     
                    }, context.CancellationToken);
                

                    _logger.LogInformation(
                        "Person updated and IAcceptTicketEvent published successfully | TicketId: {TicketId}",
                        context.Message.TicketId);
                }
                else
                {
                    await context.Publish<ICancelPersonEvent>(context.Message, context.CancellationToken);

                    _logger.LogWarning(
                        "ICancelTripEvent published because Passenger or Leader was not found | TicketId: {TicketId}, PassengerFound: {PassengerFound}, LeaderFound: {LeaderFound}",
                        context.Message.TicketId,
                        passenger is not null,
                        leader is not null);
                }

                _logger.LogInformation(
                    "SendPersonEvent processing completed | TicketId: {TicketId}",
                    context.Message.TicketId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "SendPersonEvent processing failed | TicketId: {TicketId}, Error: {ErrorMessage}",
                    context.Message.TicketId,
                    ex.Message);

                throw; // MassTransit به صورت خودکار بر اساس کانفیگ، Retry را مدیریت می‌کند
            }
        }
    }
}