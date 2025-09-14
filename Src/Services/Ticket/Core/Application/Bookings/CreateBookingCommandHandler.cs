using MediatR;
using Domain.BookingAggregate;

namespace Application.Ticketing
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, CreateBookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<CreateBookingResponse> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
        {
        
            
            // ساخت Aggregate Root Booking
            var booking =new Booking(
                tripId: command.TripId,
                passengerId: command.PassengerId,
                purchaseDate: command.PurchaseDate,
                price:new Price(command.PriceAmount)
            );

            // ذخیره در ریپوزیتوری
            await _bookingRepository.AddBookingAsync(booking, cancellationToken);
            await _bookingRepository.SaveChangesAsync(cancellationToken);

            // بازگرداندن پاسخ
            return new CreateBookingResponse(
                BookingId: booking.Id,
                TripId: booking.TripId,
                PassengerId: booking.PassengerId,
                PurchaseDate: booking.PurchaseDate,
                PriceAmount: booking.Price.Amount);
     
        }
    }


}
