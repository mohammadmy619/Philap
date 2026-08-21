using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.Implements;
using Domain.TripAggregate;
using MassTransit;
using MassTransit.NewIdProviders;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers
{
    public class SendTripEventConsumer : IConsumer<ISendTripEvent>
    {
        private readonly ILogger<SendTripEventConsumer> _logger;
        private readonly ITripRepository _tripRepository;

        public SendTripEventConsumer(
            ILogger<SendTripEventConsumer> logger,
            ITripRepository tripRepository)
        {
            _logger = logger;
            _tripRepository = tripRepository;
        }

        public async Task Consume(ConsumeContext<ISendTripEvent> context)
        {


            try
            {


                var trip = await _tripRepository.GetTripByIdAsync(
                    context.Message.TripId,
                    context.CancellationToken);

                if (trip is not null)
                {


                    _logger.LogInformation(
                        "Adding ticket to trip | TicketId: {TicketId}, TripId: {TripId}",
                        context.Message.TicketId,
                        trip.Id);

                    trip.AddTicketId(context.Message.TicketId);


                    await _tripRepository.SaveChangesAsync(context.CancellationToken);


                    await context.Publish<ISendPersonEvent>(new
                    {
                        // پراپرتی‌های IBaseTicketEvent
                        CorrelationId = context.Message.CorrelationId, // یا Guid.NewGuid() برای شروع یک Correlation جدید
                        TicketId = context.Message.TicketId,
                        TripId = context.Message.TripId,
                        PassengerId = context.Message.PassengerId,
                        CurrentState = "PersonUpdated", // وضعیت جدید را اینجا مشخص می‌کنید
                        TicketCreatedDate = context.Message.TicketCreatedDate,
                        TicketCancelDate = context.Message.TicketCancelDate,
                        Status = "Success", // یا هر وضعیت مناسب دیگری

                        LeaderId = trip.LeaderId
                    }, context.CancellationToken);

                    _logger.LogInformation(
                        "ISendPersonEvent published | TicketId: {TicketId}",
                        context.Message.TicketId);
                }
                else
                {
                    await context.Publish<ICancelTripEvent>(context.Message);

                    _logger.LogWarning(
                        "ICancelTripEvent published | TripId: {TripId}",
                        context.Message.TripId);
                }

                _logger.LogInformation(
                    "SendTripEvent processing completed | TicketId: {TicketId}, TripId: {TripId}",
                    context.Message.TicketId,
                    context.Message.TripId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "SendTripEvent processing failed | TicketId: {TicketId}, TripId: {TripId}, Error: {ErrorMessage}",
                    context.Message.TicketId,
                    context.Message.TripId,
                    ex.Message);
                throw;
            }
        }
    }
}
