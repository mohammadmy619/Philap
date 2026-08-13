using BuildingBlocks.Domain;
using Domain.BookingAggregate;
using System;

namespace Domain.TripAggregate.Events
{
    public class BookingCreatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredOn { get; }

        public Guid BookingId { get; }
        public Guid TripId { get; }
        public Guid PassengerId { get; }
        public Guid? DiscountId { get; }
        public DateTime PurchaseDate { get; }
        public Money Price { get; }
        public BookingStatus Status { get; }

        public BookingCreatedDomainEvent(
            Guid bookingId,
            Guid tripId,
            Guid passengerId,
            Guid? discountId,
            DateTime purchaseDate,
            Money priceAmount,
            BookingStatus status)
        {
            Id = Guid.NewGuid(); // تولید EventId منحصر به فرد
            OccurredOn = DateTime.UtcNow; // استفاده از UTC به جای Now
            BookingId = bookingId;
            TripId = tripId;
            PassengerId = passengerId;
            DiscountId = discountId;
            PurchaseDate = purchaseDate;
            Price = priceAmount;
            Status = status;
        }
    }
}