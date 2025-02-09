using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TripAggregate.Exceptions
{
    public class PriceNotValidExcption : DomainException
    {
        public PriceNotValidExcption(string? message= "The amount entered is not valid.", string code= "0314011") : base(message, code)
        {
        }
    }
}
