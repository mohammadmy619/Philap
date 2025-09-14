using Domain.BookingAggregate.Exceptions;

public static class BookingStatusGuard
{
    public static void GuardAgainstInvalidStatusTransition(BookingStatus currentStatus, BookingStatus newStatus)
    {
        if (!CanTransitionFrom(currentStatus, newStatus))
        {
            throw new InvalidBookingStatusTransitionException(currentStatus, newStatus);
        }
    }

       private static bool CanTransitionFrom(BookingStatus current, BookingStatus target)
        {
            // منطق تغییر وضعیت
            return (current, target) switch
            {
                (BookingStatus.Created, BookingStatus.Confirmed or BookingStatus.Cancelled) => true,
                //(BookingStatus.PendingPayment, BookingStatus.Confirmed or BookingStatus.Cancelled or BookingStatus.Expired) => true,
                (BookingStatus.Confirmed, BookingStatus.Cancelled or BookingStatus.Created) => true,
                //(BookingStatus.Completed, BookingStatus.Cancelled) => true,
                _ => current == target // اجازه برابر بودن
            };
        }
}