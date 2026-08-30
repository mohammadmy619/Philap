namespace BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;

public interface ICancelUpdatePersonEvent : IBaseTicketEvent
{
    string Reason { get; }
    DateTime CancelledDate { get; }
}
