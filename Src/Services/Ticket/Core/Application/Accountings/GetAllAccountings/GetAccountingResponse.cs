namespace Application.Accountings.GetAllAccountings
{
    public record GetAccountingResponse(
        Guid AccountingId,
        Guid BookingId,
        Guid TripId,
        Guid PassengerId,
        DateTime EntryDate,
        DateTime PurchaseDate,
        decimal PriceAmount,
        decimal PriceDiscountAmount,
        decimal PriceFinalAmount,
        string PriceCurrency,
        PaymentStatus PaymentStatus,
        string? Description);
}
