using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons.Exceptions
{
    public class StreetIsNullException : DomainException
    {
        public StreetIsNullException(string message = "Street is required.", string code = "0145001")
            : base(message, code) { }
    }

    public class CityIsNullException : DomainException
    {
        public CityIsNullException(string message = "City is required.", string code = "0145002")
            : base(message, code) { }
    }

    public class StateIsNullException : DomainException
    {
        public StateIsNullException(string message = "State is required and must be valid (2-3 characters).",
            string code = "0145003")
            : base(message, code) { }
    }

    public class ZipCodeIsNullException : DomainException
    {
        public ZipCodeIsNullException(string message = "Zip Code is required and must be exactly 5 characters.",
            string code = "0145004")
            : base(message, code) { }
    }
}
