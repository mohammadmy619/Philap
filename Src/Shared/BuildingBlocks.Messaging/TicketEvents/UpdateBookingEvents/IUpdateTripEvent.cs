namespace BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;

public interface IUpdateTripEvent : IBaseTicketEvent
{
    Guid? NewTripId { get; }
    string Status { get; }
    DateTime TicketUpdatedDate { get; }
}
