using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using Domain.AccountingAggregate.Exceptions;
using Domain.BookingAggregate.Exceptions;

namespace Domain.AccountingAggregate
{
    public class Accounting:AggregateRoot<Guid>
    {

        #region Properties
        public Guid BookingId { get; private set; }
        public Guid TripId { get; private set; }
        public Guid PassengerId { get; private set; }

        public DateTime EntryDate { get; private set; }
        public DateTime PurchaseDate { get; private set; }

        public Money BaseAmount { get; private set; }
        public Money DiscountAmount { get; private set; }
        public Money FinalAmount { get; private set; }
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
            Money baseAmount,
            Money discountAmount,
            Money finalAmount,
            PaymentStatus paymentStatus,
            string? description = null)
        {
            GuardAgainstBookingId(bookingId);
            GuardAgainstTripId(tripId);
            GuardAgainstPassengerId(passengerId);
            GuardAgainstEntryDate(entryDate);
            GuardAgainstPurchaseDate(purchaseDate);
            GuardAgainstAmounts(baseAmount, discountAmount, finalAmount);
            GuardAgainstPaymentStatus(paymentStatus);

            BookingId = bookingId;
            TripId = tripId;
            PassengerId = passengerId;
            EntryDate = entryDate;
            PurchaseDate = purchaseDate;
            BaseAmount = baseAmount;
            DiscountAmount = discountAmount;
            FinalAmount = finalAmount;
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

        private void GuardAgainstAmounts(Money baseAmount, Money discountAmount, Money finalAmount)
        {
            if (baseAmount == null)
            {
                throw new BaseAmountIsInvalidException();
            }
            if (discountAmount == null)
            {
                throw new DiscountAmountIsInvalidException();
            }
            if (finalAmount == null)
            {
                throw new FinalAmountIsInvalidException();
            }

            if (baseAmount.Amount < 0)
            {
                throw new BaseAmountIsInvalidException();
            }
            if (discountAmount.Amount < 0)
            {
                throw new DiscountAmountIsInvalidException();
            }
            if (finalAmount.Amount < 0)
            {
                throw new FinalAmountIsInvalidException();
            }

            // بررسی اینکه مقدار نهایی برابر با (پایه - تخفیف) باشد
            if (Math.Abs(finalAmount.Amount - (baseAmount.Amount - discountAmount.Amount)) > 0.01m)
            {
                throw new FinalAmountIsInvalidException("FinalAmount must equal BaseAmount minus DiscountAmount");
            }
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
