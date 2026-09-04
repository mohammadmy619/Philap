
namespace BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents
{
    public sealed record TripCancellationEvent : ITripCancellationEvent
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

        // ITripCancellationEvent Properties
        public DateTime CancelledAt { get; init; }

        // Constructor اصلی
        public TripCancellationEvent(
            Guid ticketId,
            Guid tripId,
            Guid passengerId,
            string currentState,
            DateTime ticketCreatedDate,
            string? status,
            DateTime cancelledAt)
        {
            TicketId = ticketId;
            TripId = tripId;
            PassengerId = passengerId;
            CurrentState = currentState;
            TicketCreatedDate = ticketCreatedDate;
            Status = status;


            CancelledAt = cancelledAt;
            TicketCancelDate = cancelledAt;

            CorrelationId = ticketId;
        }

        public TripCancellationEvent()
        {
        }
    }
}