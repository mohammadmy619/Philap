public record CreateBookingResponse(
    Guid BookingId,
    Guid TicketId,
    Guid TripId,
    Guid PassengerId,
    DateTime PurchaseDate,
    decimal PriceAmount);