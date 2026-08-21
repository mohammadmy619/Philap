using BuildingBlocks.Messaging.TicketEvents;

namespace BuildingBlocks.Messaging.TicketEvents.Implements
{
    public sealed record AddTicketEvent : IAddTicketEvent
    {
        // IIntegrationEvent Properties
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

        // IBaseTicketEvent Properties
        public Guid CorrelationId { get; init; }
        public Guid TicketId { get; init; }
        public Guid TripId { get; init; }
        public Guid PassengerId { get; init; }
        public string CurrentState { get; init; } = string.Empty;
        public DateTime TicketCreatedDate { get; init; }
        public DateTime TicketCancelDate { get; init; }
        public string? Status { get; init; }

        // Constructor
        public AddTicketEvent(
            Guid ticketId,
            Guid tripId,
            Guid passengerId,
            string currentState,
            DateTime ticketCreatedDate,
            DateTime ticketCancelDate,
            string? status = null)
        {
            TicketId = ticketId;
            TripId = tripId;
            PassengerId = passengerId;
            CurrentState = currentState;
            TicketCreatedDate = ticketCreatedDate;
            TicketCancelDate = ticketCancelDate;
            Status = status;

            // ⭐ CorrelationId برابر TicketId برای Saga Correlation
            CorrelationId = ticketId;
        }
    }
}