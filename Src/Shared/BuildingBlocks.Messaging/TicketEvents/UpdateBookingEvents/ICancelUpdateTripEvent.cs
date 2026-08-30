namespace BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;


public interface ICancelUpdateTripEvent : IBaseTicketEvent
{
    string Reason { get; }
    DateTime CancelledDate { get; }
}
