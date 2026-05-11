using MediatR;

namespace Application.Ticketing
{
    public record GetBookingByIdQuery(Guid BookingId) : IRequest<GetBookingResponse>;
}
