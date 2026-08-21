using Application.Mapping;
using BuildingBlocks.Domain;
using Domain.TripAggregate.Events;
using MediatR;

namespace Application.Bookings.EventHandlers;

public sealed class BookingCreatedDomainEventHandler(
    IIntegrationEventPublisher integrationEventPublisher
) : INotificationHandler<BookingCreatedDomainEvent>
{
    public async Task Handle(
        BookingCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        
        var integrationEvent = notification.ToIntegrationEvent();

        await integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
    }
}