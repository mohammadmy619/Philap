namespace BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents
{
    public interface ITripCancellationFailedEvent : IBaseTicketEvent
    {
        Guid TripId { get; }
        string Reason { get; }
    }
}
