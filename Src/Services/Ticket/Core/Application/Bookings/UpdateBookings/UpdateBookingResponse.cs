using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bookings.UpdateBookings
{

    public record UpdateBookingResponse(
        Guid BookingId,
        Guid TripId,
        Guid PassengerId,
        DateTime PurchaseDate,
        decimal PriceAmount,
        BookingStatus Status);
}

