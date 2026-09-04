using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.Implements;
using Domain.TripAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Consumers.AddTicketEvents
{

    public class CancelTripConsumer : IConsumer<ICancelPersonEvent>
    {
        private readonly ILogger<CancelTripConsumer> _logger;
        private readonly ITripRepository _tripRepository;

        public CancelTripConsumer(
            ILogger<CancelTripConsumer> logger,
            ITripRepository tripRepository)
        {
            _logger = logger;
            _tripRepository = tripRepository;
        }

        public async Task Consume(ConsumeContext<ICancelPersonEvent> context)
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


                    _logger.LogInformation(
                      "Publishing ICancelTripEvent | TicketId: {TicketId}, TripId: {TripId}",
                      context.Message.TicketId,
                      context.Message.TripId);

                    await context.Publish<ICancelTripEvent>(context.Message);
                 

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