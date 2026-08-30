using BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;

public record UpdateTripEvent(
       Guid TicketId,
       Guid TripId,
       Guid PassengerId,
       string Status,
       Guid? NewTripId = null,
       DateTime? TicketUpdatedDate = null
   ) : IUpdateTripEvent