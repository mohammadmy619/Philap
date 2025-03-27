using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BookingAggregate
{
    public class Price : ValueObject<Price>
    {

        public decimal Amount { get; private set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;


        }
    }
}
