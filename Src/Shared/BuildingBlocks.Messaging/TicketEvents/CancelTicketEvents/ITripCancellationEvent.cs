namespace BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents
{
    public interface ITripCancellationEvent : IBaseTicketEvent
    {
        Guid TripId { get; }
        DateTime CancelledAt { get; }
    }
}
