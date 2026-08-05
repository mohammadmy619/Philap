using MediatR;

namespace Application.Ticketing
{
    public record CreateBookingCommand(
       Guid TicketId,
       Guid TripId,
       Guid PassengerId,
       DateTime PurchaseDate,
       decimal PriceAmount,
       string PriceCurrency,
        string DiscountCode
       ) : IRequest<CreateBookingResponse>;


}


