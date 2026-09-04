using BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;

public sealed record UpdateTripEvent : IUpdateTripEvent
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

    // IUpdateTripEvent Properties
    public Guid? NewTripId { get; init; }
    public DateTime TicketUpdatedDate { get; init; }

    // Constructor اصلی
    public UpdateTripEvent(
        Guid ticketId,
        Guid tripId,
        Guid passengerId,
        string currentState,
        DateTime ticketCreatedDate,
        DateTime ticketCancelDate,
        string? status = null,
        Guid? newTripId = null,
        DateTime ticketUpdatedDate = default)
    {
        TicketId = ticketId;
        TripId = tripId;
        PassengerId = passengerId;
        CurrentState = currentState;
        TicketCreatedDate = ticketCreatedDate;
        TicketCancelDate = ticketCancelDate;
        Status = status;

        // مقداردهی فیلدهای اختصاصی UpdateTripEvent
        NewTripId = newTripId;
        TicketUpdatedDate = ticketUpdatedDate == default ? DateTime.UtcNow : ticketUpdatedDate;

        // ⭐ CorrelationId برابر TicketId برای Saga Correlation
        CorrelationId = ticketId;
    }

    // Constructor بدون پارامتر برای سازگاری با Serializerها (مثل System.Text.Json یا JSON.NET)
    public UpdateTripEvent()
    {
    }
}