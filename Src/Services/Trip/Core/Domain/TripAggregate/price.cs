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
        public Price(decimal Amount, string Currency)
        {
            GuardAgainstPrice(Amount);
            GuardAgainstInvalidCurrency(Currency);
            this.Amount = Amount;
            this.Currency = Currency;
        }

        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        #region GuardAgainst
        private static void GuardAgainstPrice(decimal Amount)
        {
            // Check if price is less than or equal to zero  
            if (Amount <= 0)
            {
                throw new InvalidPriceException();
            }
        }
        private static void GuardAgainstInvalidCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new InvalidCurrencyException("Currency code cannot be null or empty.", nameof(currency));
            }

            if (currency.Length != 3 || !currency.All(char.IsUpper))
            {
                throw new InvalidCurrencyException("Currency code must be exactly 3 uppercase letters.", nameof(currency));
            }
        }

        #endregion
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
