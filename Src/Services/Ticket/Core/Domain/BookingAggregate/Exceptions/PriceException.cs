using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BookingAggregate.Exceptions
{
    public class PriceIsInvalidException : DomainException
    {
        public PriceIsInvalidException(string message = "Price must be a valid amount.", string code = "0527001")
            : base(message, code) { }
    }
}
