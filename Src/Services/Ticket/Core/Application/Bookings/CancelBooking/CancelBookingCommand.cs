using MediatR;

namespace Application.Ticketing
{
    // کامند برای لغو کردن رزرو
    public record CancelBookingCommand(Guid BookingId) : IRequest<CancelBookingResponse>;
}