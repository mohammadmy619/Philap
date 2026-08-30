using Application.Mapping;
using BuildingBlocks.Domain;
using MediatR;
using Microsoft.Extensions.Logging; 

namespace Application.Bookings.EventHandlers
{
    public sealed class BookingUpdatedDomainEventHandler(
        IIntegrationEventPublisher integrationEventPublisher,
        ILogger<BookingUpdatedDomainEventHandler> logger 
    ) : INotificationHandler<BookingUpdatedEvent>
    {
        public async Task Handle(
            BookingUpdatedEvent notification,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Received domain event {EventName} for BookingId: {BookingId}",
                nameof(BookingUpdatedEvent),
                notification.BookingId);

            try
            {
                var integrationEvent = notification.ToIntegrationEvent();

                await integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);

                logger.LogInformation(
                    "Successfully published integration event {IntegrationEventType} for BookingId: {BookingId}",
                    integrationEvent.GetType().Name,
                    notification.BookingId);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Failed to publish integration event for BookingId: {BookingId}",
                    notification.BookingId);

                throw;
            }
        }
    }
}