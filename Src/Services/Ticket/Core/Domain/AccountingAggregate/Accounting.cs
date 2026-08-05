using BuildingBlocks.Domain;
using Domain.AccountingAggregate.Exceptions;
using Domain.BookingAggregate;
using Domain.BookingAggregate.Exceptions;



namespace Domain.AccountingAggregate
{
    public class Accounting : AggregateRoot<Guid>
    {

        #region Properties
        public Guid BookingId { get; private set; }
        public Guid TripId { get; private set; }
        public Guid PassengerId { get; private set; }

        public DateTime EntryDate { get; private set; }
        public DateTime PurchaseDate { get; private set; }

        public Money Price { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public string? Description { get; private set; }
        #endregion
        #region Constructor
        // Constructor برای EF Core
        private Accounting() { }

        public Accounting(
            Guid bookingId,
            Guid tripId,
            Guid passengerId,
            DateTime entryDate,
            DateTime purchaseDate,
            Money price,

            PaymentStatus paymentStatus,
            string? description = null)
        {
            GuardAgainstBookingId(bookingId);
            GuardAgainstTripId(tripId);
            GuardAgainstPassengerId(passengerId);
            GuardAgainstEntryDate(entryDate);
            GuardAgainstPurchaseDate(purchaseDate);
            GuardAgainstPrice(price);
            GuardAgainstPaymentStatus(paymentStatus);

            BookingId = bookingId;
            TripId = tripId;
            PassengerId = passengerId;
            EntryDate = entryDate;
            PurchaseDate = purchaseDate;
            Price = price;
            PaymentStatus = paymentStatus;
            Description = description;
        }
        #endregion
        #region Guard Methods
        private void GuardAgainstBookingId(Guid bookingId)
        {
            if (bookingId == Guid.Empty)
            {
                throw new BookingIdIsNullException();
            }
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

        private void GuardAgainstEntryDate(DateTime entryDate)
        {
            if (entryDate == default(DateTime))
            {
                throw new EntryDateIsInvalidException();
            }
            if (entryDate > DateTime.Now)
            {
                throw new EntryDateIsInvalidException("EntryDate cannot be in the future");
            }
        }

        private void GuardAgainstPurchaseDate(DateTime purchaseDate)
        {
            if (purchaseDate == default(DateTime))
            {
                throw new PurchaseDateIsInvalidException();
            }
            if (purchaseDate > DateTime.Now)
            {
                throw new PurchaseDateIsInvalidException();
            }
        }

        private void GuardAgainstPrice(Money price)
        {
            if (price == null)
                throw new PriceIsInvalidException();

            // چک کردن Invariant کلیدی: مبلغ نهایی نباید منفی باشد
            if (price.FinalAmount < 0)
                throw new PriceIsInvalidException("Final amount in accounting cannot be negative.");
        }


        private void GuardAgainstPaymentStatus(PaymentStatus paymentStatus)
        {
            if (!Enum.IsDefined(typeof(PaymentStatus), paymentStatus))
            {
                throw new PaymentStatusIsInvalidException();
            }
        }
        #endregion




    }
}
