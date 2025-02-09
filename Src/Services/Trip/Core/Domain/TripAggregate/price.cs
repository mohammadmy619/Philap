using BuildingBlocks.Domain;
using Domain.TripAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TripAggregate
{
   public class Price : ValueObject<Price>
    {
        public Price(decimal price)
        {
            GuardAgainstPrice(price);
            this.price = price;
        }

        public decimal price { get; private set; }


        #region GuardAgainst
        private static void GuardAgainstPrice(decimal price)
        {
            // Check if price is less than or equal to zero  
            if (price <= 0)
            {
                throw new InvalidPriceException();
            }
        }


        #endregion
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return price;

        }
    }
}
