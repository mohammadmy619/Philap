using BuildingBlocks.Domain;

namespace Domain.DomainServices
{
    public class PricingResult
    {
        public Money BaseAmount { get; }
        public Money DiscountAmount { get; }
        public Money FinalAmount { get; }

        public PricingResult(Money baseAmount, Money discountAmount, Money finalAmount)
        {
            BaseAmount = baseAmount;
            DiscountAmount = discountAmount;
            FinalAmount = finalAmount;
        }
    }
}
