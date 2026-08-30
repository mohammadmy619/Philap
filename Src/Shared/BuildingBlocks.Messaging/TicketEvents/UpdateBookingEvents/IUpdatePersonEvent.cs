namespace BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;

public interface IUpdatePersonEvent : IBaseTicketEvent
{
    Guid? LeaderId { get; }
    Guid? NewPassengerId { get; }
    string Status { get; }
    DateTime TicketUpdatedDate { get; }
}
