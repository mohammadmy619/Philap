namespace BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;

// ==========================================
// 5. رویداد تایید و نهایی‌سازی به روزرسانی تیکت
// ==========================================
public interface IAcceptUpdateTicketEvent : IBaseTicketEvent
{
    decimal PriceAmount { get; }
    string PriceCurrency { get; }
    string Status { get; }
    DateTime CompletedDate { get; }
}

