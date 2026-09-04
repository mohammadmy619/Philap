namespace BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents
{
    public interface ICancellationConfirmedEvent : IBaseTicketEvent
    {
        Guid PassengerId { get; }
        string Reason { get; }
    }
}
