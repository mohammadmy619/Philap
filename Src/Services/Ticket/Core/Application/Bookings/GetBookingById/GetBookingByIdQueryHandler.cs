using Application.Bookings.GetBookingById;
using Domain.BookingAggregate;
using MediatR;

namespace Application.Ticketing
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, GetBookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBookingByIdQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<GetBookingResponse> Handle(GetBookingByIdQuery query, CancellationToken cancellationToken)
        {
            // 1. دریافت رزرو از ریپوزیتوری
            var booking = await _bookingRepository.GetBookingByIdAsync(query.BookingId, cancellationToken)
                ?? throw new InvalidOperationException($"Booking with Id {query.BookingId} not found");

            // 2. مپ کردن دامین به Response
            // توجه: چون Price از نوع Money است، باید Amount و Currency آن را جداگانه استخراج کنید
            return new GetBookingResponse(
                BookingId: booking.Id,
                TripId: booking.TripId,
                PassengerId: booking.PassengerId,
                PurchaseDate: booking.PurchaseDate,
                PriceAmount: booking.Price.Amount,
                PriceCurrency: booking.Price.Currency,
                Status: booking.Status
            );
        }
    }
}