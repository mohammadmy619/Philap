using BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;

public sealed record UpdateBookingEvent : IUpdateBookingEvent
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

    // IUpdateBookingEvent Properties
    public decimal PriceAmount { get; init; }
    public string PriceCurrency { get; init; } = string.Empty;

    
    public string? PreviousState { get; init; }
    public DateTime TicketUpdatedDate { get; init; }

    // Constructor
    public UpdateBookingEvent(
        Guid ticketId,
        Guid tripId,
        Guid passengerId,
        string currentState,
        DateTime ticketCreatedDate,
        DateTime ticketCancelDate,
        decimal priceAmount,
        string priceCurrency,
        string? status = null,
        string? previousState = null,
        DateTime ticketUpdatedDate = default)
    {
        TicketId = ticketId;
        TripId = tripId;
        PassengerId = passengerId;
        CurrentState = currentState;
        TicketCreatedDate = ticketCreatedDate;
        TicketCancelDate = ticketCancelDate;
        Status = status;
        PriceAmount = priceAmount;
        PriceCurrency = priceCurrency;

        // مقداردهی فیلدهای جدید
        PreviousState = previousState;
        TicketUpdatedDate = ticketUpdatedDate == default ? DateTime.UtcNow : ticketUpdatedDate;

        // ⭐ CorrelationId برابر TicketId برای Saga Correlation
        CorrelationId = ticketId;
    }

    public UpdateBookingEvent()
    {
    }
}