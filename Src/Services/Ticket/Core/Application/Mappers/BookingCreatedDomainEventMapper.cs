using BuildingBlocks.Messaging.TicketEvents.Implements;
using Domain.BookingAggregate;
using Domain.TripAggregate.Events;

namespace Application.Mapping
{
    public static class BookingCreatedDomainEventMapper
    {
        public static AddTicketEvent ToIntegrationEvent(
            this BookingCreatedDomainEvent domainEvent)
        {
            return new AddTicketEvent(
                ticketId: domainEvent.BookingId,
                tripId: domainEvent.TripId,
                passengerId: domainEvent.PassengerId,
                currentState: domainEvent.Status.ToString(),
                ticketCreatedDate: domainEvent.PurchaseDate,
                ticketCancelDate: DateTime.MinValue, // صریح‌تر از default
                status: domainEvent.Status.ToString()
            );
        }
    }
}