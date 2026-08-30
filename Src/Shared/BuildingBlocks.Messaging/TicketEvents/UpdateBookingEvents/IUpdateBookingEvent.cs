namespace BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;
public interface IUpdateBookingEvent : IBaseTicketEvent
{
    decimal PriceAmount { get; }
    string PriceCurrency { get; }

    // ✨ فیلدهای جدید برای ردیابی تغییرات
    string? PreviousState { get; }
    DateTime TicketUpdatedDate { get; }
}
