using BuildingBlocks.Domain;
using Domain.BookingAggregate;

namespace Domain.BookingAggregate.Events
{
    public class BookingCancelledEvent(
        Guid ticketId,
        Guid tripId,
        Guid passengerId,
        DateTime purchaseDate,
        Money price,
        BookingStatus status,
        BookingStatus oldStatus
    ) : IDomainEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public Guid TicketId { get; } = ticketId;
        public Guid TripId { get; } = tripId;
        public Guid PassengerId { get; } = passengerId;
        public DateTime PurchaseDate { get; } = purchaseDate;
        public Money Price { get; } = price;
        public BookingStatus Status { get; } = status;
        public BookingStatus OldStatus { get; } = oldStatus;
    }
}