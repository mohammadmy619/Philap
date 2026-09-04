namespace BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents
{
    public interface IPersonCancellationEvent : IBaseTicketEvent
    {
        Guid PassengerId { get; }
        DateTime CancelledAt { get; }
    }
}
