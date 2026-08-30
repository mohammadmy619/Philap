using Application.Mapping;
using BuildingBlocks.Domain;
using Domain.TripAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging; // ۱. اضافه کردن نام‌فضای لاگر

namespace Application.Bookings.EventHandlers;

public sealed class BookingCreatedDomainEventHandler(
    IIntegrationEventPublisher integrationEventPublisher,
    ILogger<BookingCreatedDomainEventHandler> logger // ۲. تزریق لاگر در سازنده
) : INotificationHandler<BookingCreatedEvent>
{
    public async Task Handle(
        BookingCreatedEvent notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Received domain event {EventName} for BookingId: {BookingId}",
            nameof(BookingCreatedEvent),
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