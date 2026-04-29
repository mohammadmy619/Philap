namespace Application.Ticketing
{

    // پاسخ عملیات لغو
    public record CancelBookingResponse(
        Guid BookingId,
        BookingStatus PreviousStatus,
        BookingStatus CurrentStatus,
        DateTime CancelledAt);
}
