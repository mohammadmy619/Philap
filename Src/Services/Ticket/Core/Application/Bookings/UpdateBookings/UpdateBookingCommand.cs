using MediatR;

namespace Application.Ticketing
{
    // کامند برای آپدیت اطلاعات رزرو
    public record UpdateBookingCommand(
        Guid BookingId,
        Guid TripId,
        Guid PassengerId,
        DateTime PurchaseDate,
        decimal PriceAmount,
        string PriceCurrency) : IRequest<UpdateBookingResponse>;
}
