using MediatR;

namespace Application.Bookings.CreateBookings
{
    public record CreateBookingCommand(
       Guid TicketId,
       Guid TripId,
       Guid PassengerId,
       DateTime PurchaseDate,
       decimal PriceAmount,
       string PriceCurrency,
       string PriceDescription = null) : IRequest<CreateBookingResponse>;


}
