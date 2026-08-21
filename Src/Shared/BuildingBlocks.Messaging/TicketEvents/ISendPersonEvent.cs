namespace BuildingBlocks.Messaging.TicketEvents
{
    public interface ISendPersonEvent : IBaseTicketEvent
    {
        public Guid LeaderId { get; }
    }
}
