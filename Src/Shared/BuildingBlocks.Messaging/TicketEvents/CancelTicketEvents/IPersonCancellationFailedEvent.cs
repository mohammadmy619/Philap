namespace BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents
{
    public interface IPersonCancellationFailedEvent : IBaseTicketEvent
    {
        Guid PassengerId { get; }
        string Reason { get; }
    }
}
