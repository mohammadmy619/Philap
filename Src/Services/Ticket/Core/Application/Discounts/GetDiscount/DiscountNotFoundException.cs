namespace Domain.DiscountAggregate.Exceptions
{
    public class DiscountNotFoundException : Exception
    {
        public DiscountNotFoundException(Guid discountId)
            : base($"Discount with Id {discountId} not found.")
        {
        }
    }
}