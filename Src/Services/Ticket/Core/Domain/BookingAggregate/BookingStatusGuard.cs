public static class BookingStatusGuard
{
    public static void GuardAgainstInvalidStatusTransition(BookingStatus currentStatus, BookingStatus newStatus)
    {
        if (!IsValidTransition(currentStatus, newStatus))
        {
            throw new InvalidBookingStatusTransitionException(currentStatus, newStatus);
        }
    }

    private static bool IsValidTransition(BookingStatus current, BookingStatus next)
    {
        return (current, next) switch
        {
            (BookingStatus.Created, BookingStatus.Confirmed) => true,
            (BookingStatus.Created, BookingStatus.Cancelled) => true,
            (BookingStatus.Created, BookingStatus.PendingPayment) => true,
            (BookingStatus.Created, BookingStatus.Expired) => true,

            (BookingStatus.PendingPayment, BookingStatus.Confirmed) => true,
            (BookingStatus.PendingPayment, BookingStatus.Cancelled) => true,
            (BookingStatus.PendingPayment, BookingStatus.Expired) => true,

            (BookingStatus.Confirmed, BookingStatus.Cancelled) => true,
            (BookingStatus.Confirmed, BookingStatus.Completed) => true,

            (BookingStatus.Completed, BookingStatus.Cancelled) => true,

            _ => false
        };
    }
}