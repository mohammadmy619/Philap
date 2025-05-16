using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TripAggregate.Exceptions;

    public class PriceNotValidExcption : DomainException
    {
        public PriceNotValidExcption(string? message= "The amount entered is not valid.", string code= "0314011") : base(message, code)
        {
        }
    }
    public class InvalidCurrencyException : DomainException
    {
        public InvalidCurrencyException(string? message = "Invalid currency code. Must be 3 uppercase letters.", string code = "0314012")
            : base(message, code)
        {
        }
    }

