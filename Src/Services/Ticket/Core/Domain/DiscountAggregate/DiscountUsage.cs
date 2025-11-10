using BuildingBlocks.Domain;
using Domain.DiscountAggregate.Exceptions;

public class DiscountUsage : Entity<Guid>
{
    public Guid DiscountId { get; private set; }
    public Guid? BookingId { get; private set; }
    public Guid? PassengerId { get; private set; }
    public Guid? TripId { get; private set; }
    public decimal AppliedAmount { get; private set; }
    public DateTime UsedAt { get; private set; }

    #region Constructor
    private DiscountUsage() { }
    public DiscountUsage(Guid discountId, Guid? bookingId, Guid? passengerId, Guid? tripId, decimal appliedAmount)
    {
        Id = Guid.NewGuid();
        DiscountId = discountId;
        PassengerId = passengerId;
        BookingId = bookingId;
        TripId = tripId;
        AppliedAmount = appliedAmount;
        UsedAt = DateTime.UtcNow;
    }
    #endregion

    #region Methods
    private void GuardAgainstAppliedAmount(decimal appliedAmount)
    {
        if (appliedAmount < 0)
        {
            throw new DiscountUsageIdIsInvalidException("Applied amount cannot be negative.");
        }
    }
    #endregion


}
