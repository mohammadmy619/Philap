using MediatR;

namespace Application.Bookings.GetBookingById
{
    public record GetBookingByIdQuery(Guid BookingId) : IRequest<GetBookingResponse>;
}
