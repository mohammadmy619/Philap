using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace Domain.TripAggregate.Events
{
    public class BookingCreatedDomainEvent(
      Guid bookingId,
      Guid tripId,
      Guid passengerId,
      DateTime purchaseDate,
      decimal priceAmount,
      BookingStatus Status
          ) : IDomainEvent
    {
        public Guid BookingId { get; } = bookingId;
        public Guid TripId { get; } = tripId;
        public Guid PassengerId { get; } = passengerId;
        public DateTime PurchaseDate { get; } = purchaseDate;
        public decimal PriceAmount { get; } = priceAmount;
        public BookingStatus Status { get;  }= Status;
        public DateTime OccurredOn => DateTime.Now;






    }
}
