using BuildingBlocks.Domain;
using Domain.BookingAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BookingAggregate
{
    public class Price : ValueObject<Price>
    {
        public Price(decimal amount)
        {
            GuardAgainstPrice(amount);
            Amount =amount;
        }
        public decimal Amount { get; private set; }

        #region GuardAgainst
        private static void GuardAgainstPrice(decimal Amount)
        {
            // Check if price is less than or equal to zero  
            if (Amount <= 0)
            {
                throw new PriceIsInvalidException();
            }
        }
        #endregion
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;


        }
    }
}
