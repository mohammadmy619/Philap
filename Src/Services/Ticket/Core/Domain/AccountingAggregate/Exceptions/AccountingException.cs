using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace Domain.AccountingAggregate.Exceptions
{
    public class BookingIdIsNullException : DomainException
    {
        public BookingIdIsNullException(string message = "Booking ID is required.", string code = "0517002")
            : base(message, code) { }
    }

    public class EntryDateIsInvalidException : DomainException
    {
        public EntryDateIsInvalidException(string message = "Entry Date is invalid.", string code = "0517003")
            : base(message, code) { }
    }

    public class BaseAmountIsInvalidException : DomainException
    {
        public BaseAmountIsInvalidException(string message = "Base Amount is invalid.", string code = "0517004")
            : base(message, code) { }
    }

    public class DiscountAmountIsInvalidException : DomainException
    {
        public DiscountAmountIsInvalidException(string message = "Discount Amount is invalid.", string code = "0517005")
            : base(message, code) { }
    }

    public class FinalAmountIsInvalidException : DomainException
    {
        public FinalAmountIsInvalidException(string message = "Final Amount is invalid.", string code = "0517006")
            : base(message, code) { }
    }

    public class PaymentStatusIsInvalidException : DomainException
    {
        public PaymentStatusIsInvalidException(string message = "Payment Status is invalid.", string code = "0517007")
            : base(message, code) { }
    }
}
