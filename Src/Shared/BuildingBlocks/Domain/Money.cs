using BuildingBlocks.Domain;
using BuildingBlocks.Exeptions;

namespace Domain.BookingAggregate
{
    public class Money : ValueObject<Money>
    {
        public decimal Amount { get; }
        public decimal DiscountAmount { get; }
        public decimal FinalAmount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency = "IRR")
        {
            GuardAgainstAmount(amount);
            GuardAgainstCurrency(currency);

            Amount = Math.Round(amount, 2);
            DiscountAmount = 0m;
            FinalAmount = Amount;
            Currency = currency.ToUpperInvariant();
        }

        public Money(decimal amount, decimal discountAmount, string currency = "IRR")
        {
            GuardAgainstAmount(amount);
            GuardAgainstCurrency(currency);
            GuardAgainstDiscountAmount(discountAmount, amount);

            Amount = Math.Round(amount, 2);
            DiscountAmount = Math.Round(discountAmount, 2);
            FinalAmount = Math.Round(amount - discountAmount, 2);
            Currency = currency.ToUpperInvariant();
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return DiscountAmount;
            yield return FinalAmount;
            yield return Currency;
        }

        #region Guard Methods

        private void GuardAgainstAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new AmountIsInvalidException();
            }
        }

        private void GuardAgainstCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new CurrencyIsInvalidException();
            }
        }

        private void GuardAgainstDiscountAmount(decimal discountAmount, decimal baseAmount)
        {
            if (discountAmount < 0)
            {
                throw new AmountIsInvalidException("Discount amount cannot be negative.");
            }

            if (discountAmount > baseAmount)
            {
                throw new AmountIsInvalidException("Discount amount cannot be greater than base amount.");
            }
        }

        #endregion
    }
}
