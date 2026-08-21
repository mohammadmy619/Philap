using BuildingBlocks.Messaging.TicketEvents;
using Domain.TripAggregate;
using Infrastructure.Consumers;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Consumers
{
    public class CancelTicketEventConsumer : IConsumer<ICancelTicketEvent>
    {
        private readonly ILogger<CancelTicketEventConsumer> _logger;
        private readonly ITripRepository _tripRepository;

        public CancelTicketEventConsumer(
            ILogger<CancelTicketEventConsumer> logger,
            ITripRepository tripRepository)
        {
            _logger = logger;
            _tripRepository = tripRepository;
        }

        public async Task Consume(ConsumeContext<ICancelTicketEvent> context)
        {
            try
            {
                var trip = await _tripRepository.GetTripByIdAsync(
                    context.Message.TripId,
                    context.CancellationToken);

                if (trip is not null)
                {
                    _logger.LogInformation(
                        "Canceling ticket for trip | TicketId: {TicketId}, TripId: {TripId}",
                        context.Message.TicketId,
                        trip.Id);

                    trip.RemoveTicketId(context.Message.TicketId);

                    await _tripRepository.SaveChangesAsync(context.CancellationToken);


                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "CancelTripEvent processing failed | TicketId: {TicketId}, TripId: {TripId}, Error: {ErrorMessage}",
                    context.Message.TicketId,
                    context.Message.TripId,
                    ex.Message);

                throw; // MassTransit عملیات Retry را مدیریت می‌کند
            }
        }
    }
}
