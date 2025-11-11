using BuildingBlocks.Domain;
using Domain.AccountingAggregate.Exceptions;

public class Money : ValueObject<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "IRR")
    {
        GuardAgainstAmount(amount);
        GuardAgainstCurrency(currency);

        if (amount < 0) throw new ArgumentException("Amount cannot be negative");
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required");

        Amount = Math.Round(amount, 2);
        Currency = currency.ToUpper();
    }

    public override IEnumerable<object> GetEqualityComponents()
    {

        yield return Amount;
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
    #endregion

}
