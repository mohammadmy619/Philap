using BuildingBlocks.Domain;
using Domain.BookingAggregate;

public record BookingUpdatedEvent(
    Guid BookingId,
    Guid TripId,
    Guid PassengerId,
    Guid? DiscountId,
    DateTime PurchaseDate,
    Money Price,
    BookingStatus Status,
    BookingStatus? PreviousStatus = null // برای ردیابی تغییر وضعیت
) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}