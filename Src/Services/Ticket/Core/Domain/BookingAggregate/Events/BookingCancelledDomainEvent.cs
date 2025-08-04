using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace Domain.BookingAggregate.Events
{
    public class BookingCancelledDomainEvent(
      Guid ticketId,
      Guid tripId,
      Guid passengerId,
      DateTime purchaseDate,
      decimal priceAmount,
      BookingStatus Status,
       BookingStatus OldStatus
          ) : IDomainEvent
    {
   
        public Guid TicketId { get; } = ticketId;
        public Guid TripId { get; } = tripId;
        public Guid PassengerId { get; } = passengerId;
        public DateTime PurchaseDate { get; } = purchaseDate;
        public decimal PriceAmount { get; } = priceAmount;
        public BookingStatus Status { get; } = Status;
        public BookingStatus OldStatus { get; } = OldStatus;
        public DateTime OccurredOn => DateTime.Now;






    }
}
