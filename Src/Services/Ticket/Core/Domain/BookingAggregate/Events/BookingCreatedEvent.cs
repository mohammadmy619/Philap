using BuildingBlocks.Domain;
using Domain.BookingAggregate;
using System;

namespace Domain.TripAggregate.Events
{
    public record BookingCreatedEvent(
        Guid BookingId,
        Guid TripId,
        Guid PassengerId,
        Guid? DiscountId,
        DateTime PurchaseDate,
        Money Price,
        BookingStatus Status
    ) : IDomainEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
