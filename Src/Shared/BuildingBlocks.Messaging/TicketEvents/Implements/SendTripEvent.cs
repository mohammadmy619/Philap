using BuildingBlocks.Messaging.TicketEvents;

namespace BuildingBlocks.Messaging.TicketEvents.Implements
{
    public sealed record SendTripEvent(
        Guid TicketId,
        Guid TripId,
        Guid PassengerId,
        string CurrentState,
        DateTime TicketCreatedDate,
        DateTime TicketCancelDate,
        string? Status = null
    ) : ISendTripEvent
    {
        // IIntegrationEvent
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

        // ⭐ CorrelationId برابر TicketId برای Saga Correlation
        public Guid CorrelationId => TicketId;
    }
}
