using BuildingBlocks.Domain;
using Domain.BookingAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BookingAggregate
{
    public class Booking : AggregateRoot<Guid>
    {
        #region Properties  

        public Guid TripId { get; private set; }
        public Guid TicketId { get; private set; }
        public Guid PassengerId { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public Price Price { get; private set; }

        #endregion

        #region Constructor  

        public Booking( Guid tripId, Guid ticketId, Guid passengerId, DateTime purchaseDate, Price price)
            
        {
            GuardAgainstTripId(tripId);
            GuardAgainstTicketId(ticketId);
            GuardAgainstPassengerId(passengerId);
            GuardAgainstPurchaseDate(purchaseDate);
            GuardAgainstPrice(price);
            TripId = tripId;
            TicketId = ticketId;
            PassengerId = passengerId;
            PurchaseDate = purchaseDate;
            Price = price;
        }

        #endregion

        #region Guard Methods  

        private void GuardAgainstTripId(Guid tripId)
        {
            if (tripId == Guid.Empty)
            {
                throw new TripIdIsNullException();
            }
        }

        private void GuardAgainstTicketId(Guid ticketId)
        {
            if (ticketId == Guid.Empty)
            {
                throw new TicketIdIsNullException();
            }
        }

        private void GuardAgainstPassengerId(Guid passengerId)
        {
            if (passengerId == Guid.Empty)
            {
                throw new PassengerIdIsNullException();
            }
        }

        private void GuardAgainstPurchaseDate(DateTime purchaseDate)
        {
            if (purchaseDate > DateTime.Now)
            {
                throw new PurchaseDateIsInvalidException();
            }
        }

        private void GuardAgainstPrice(Price price)
        {
            
            if (price.Amount <= 0)  
            {
                throw new PriceIsInvalidException();
            }
        }

        #endregion
    }
}
