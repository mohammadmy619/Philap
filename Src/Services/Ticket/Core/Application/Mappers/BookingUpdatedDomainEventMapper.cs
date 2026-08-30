using BuildingBlocks.Messaging.TicketEvents.Implements;
using Domain.TripAggregate.Events; // یا نام‌فضای BookingUpdatedEvent شما

namespace Application.Mapping
{
    public static class BookingUpdatedDomainEventMapper
    {
        public static UpdateBookingEvent ToIntegrationEvent(
            this BookingUpdatedEvent domainEvent)
        {
            return new UpdateBookingEvent(
                ticketId: domainEvent.BookingId,
                tripId: domainEvent.TripId,
                passengerId: domainEvent.PassengerId,
                currentState: domainEvent.Status.ToString(),
                ticketCreatedDate: domainEvent.PurchaseDate,
                ticketCancelDate: DateTime.MinValue,
                priceAmount: domainEvent.Price.Amount,
                priceCurrency: domainEvent.Price.Currency,
                status: domainEvent.Status.ToString(),
                previousState: domainEvent.PreviousStatus?.ToString(),
                ticketUpdatedDate: domainEvent.OccurredOn
            );
        }
    }
}