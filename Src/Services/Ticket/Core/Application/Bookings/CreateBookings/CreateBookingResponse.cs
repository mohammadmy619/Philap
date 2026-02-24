public record CreateBookingResponse(
    Guid BookingId,
    Guid TripId,
    Guid PassengerId,
    DateTime PurchaseDate,
    decimal PriceAmount);