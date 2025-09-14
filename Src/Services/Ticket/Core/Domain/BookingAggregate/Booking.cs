using BuildingBlocks.Domain;
using Domain.BookingAggregate.Events;
using Domain.BookingAggregate.Exceptions;
using Domain.TripAggregate.Events;

namespace Domain.BookingAggregate
{
    public class Booking : AggregateRoot<Guid>
    {
        #region Properties  
        public Guid TripId { get; private set; }
        public Guid PassengerId { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public Price Price { get; private set; }
        public BookingStatus Status { get; private set; }
        #endregion

        #region Constructor  
        // Constructor برای EF Core
        private Booking() { }

        public Booking(Guid tripId, Guid passengerId, DateTime purchaseDate, Price price)

        {
            GuardAgainstTripId(tripId);
            //GuardAgainstTicketId(ticketId);
            GuardAgainstPassengerId(passengerId);
            GuardAgainstPurchaseDate(purchaseDate);
            GuardAgainstPrice(price);

            TripId = tripId;
            //TicketId = ticketId;
            PassengerId = passengerId;
            PurchaseDate = purchaseDate;
            Price = price;
            Status = BookingStatus.Created;
            AddEvent(new BookingCreatedDomainEvent(
                         Id,
                         //ticketId,
                         tripId,
                         passengerId,
                         PurchaseDate,
                         price.Amount,
                         Status

                     ));
        }

        #endregion
        #region Method
        // متد برای کنسل کردن رزرو
        public void Cancel()
        {
            if (Status != BookingStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed bookings can be cancelled");

            var oldStatus = Status;
            Status = BookingStatus.Cancelled;

            // اضافه کردن Domain Event برای کنسل کردن
            AddEvent(new BookingCancelledDomainEvent(
                Id,
                TripId,
                PassengerId,
                PurchaseDate,
                Price.Amount,
                Status,
                oldStatus
            ));
        }

        #endregion

        #region Guard Methods  
        public void ChangeStatus(BookingStatus newStatus)
        {
            BookingStatusGuard.GuardAgainstInvalidStatusTransition(Status, newStatus);
        }


      
    
        private void GuardAgainstTripId(Guid tripId)
        {
            if (tripId == Guid.Empty)
            {
                throw new TripIdIsNullException();
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
