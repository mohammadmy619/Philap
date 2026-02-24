using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bookings.GetBookingById
{

    // پاسخ شامل تمام پراپرتی‌های مورد نیاز
    public record GetBookingResponse(
        Guid BookingId,
        Guid TripId,
        Guid PassengerId,
        DateTime PurchaseDate,
        decimal PriceAmount,
        string PriceCurrency,
        BookingStatus Status);
}
