public record GetDiscountUsageResponse(
    Guid UsageId,
    Guid DiscountId,
    Guid? BookingId,
    Guid? PassengerId,
    Guid? TripId,
    decimal AppliedAmount,
    DateTime UsedAt
);