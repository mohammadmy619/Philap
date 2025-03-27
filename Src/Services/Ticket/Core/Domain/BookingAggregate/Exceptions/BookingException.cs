using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BookingAggregate.Exceptions
{
    public class TripIdIsNullException : DomainException
    {
        public TripIdIsNullException(string message = "Trip ID is required.", string code = "0517001")
            : base(message, code) { }
    }

    public class TicketIdIsNullException : DomainException
    {
        public TicketIdIsNullException(string message = "Ticket ID is required.", string code = "0517002")
            : base(message, code) { }
    }

    public class PassengerIdIsNullException : DomainException
    {
        public PassengerIdIsNullException(string message = "Passenger ID is required.", string code = "0517003")
            : base(message, code) { }
    }

    public class PurchaseDateIsInvalidException : DomainException
    {
        public PurchaseDateIsInvalidException(string message = "Purchase date cannot be in the future.", string code = "0517004")
            : base(message, code) { }
    }


}
