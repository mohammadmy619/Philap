using Domain.BookingAggregate.Exceptions;

public static class BookingStatusGuard
{
    public static void GuardAgainstInvalidStatusTransition(
        BookingStatus currentStatus,
        BookingStatus newStatus)
    {
        if (!CanTransitionFrom(currentStatus, newStatus))
        {
            throw new InvalidBookingStatusTransitionException(
                currentStatus,
                newStatus);
        }
    }

    private static bool CanTransitionFrom(
        BookingStatus current,
        BookingStatus target)
    {
        if (current == target)
            return true;

        if (current == BookingStatus.Created)
            return target == BookingStatus.Confirmed
                || target == BookingStatus.Cancelled;

        if (current == BookingStatus.Confirmed)
            return target == BookingStatus.Cancelled
                || target == BookingStatus.Created;

        return false;
    }
}