using BuildingBlocks.Domain;
using Domain.BookingAggregate;
using MediatR;

namespace Application.Ticketing
{
    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, UpdateBookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public UpdateBookingCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<UpdateBookingResponse> Handle(UpdateBookingCommand command, CancellationToken cancellationToken)
        {
         
            var booking = await _bookingRepository.GetBookingByIdAsync(command.BookingId, cancellationToken)
                ?? throw new InvalidOperationException($"Booking with Id {command.BookingId} not found");
        
          
            if (booking.Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException("Cancelled bookings cannot be updated");
            }

            var newPrice = new Money(command.PriceAmount, command.PriceCurrency);
            booking.Update(
                command.TripId,
                command.PassengerId,
                command.PurchaseDate,
                newPrice
            );

            await _bookingRepository.SaveChangesAsync(cancellationToken);

            return new UpdateBookingResponse(
                BookingId: booking.Id,
                TripId: booking.TripId,
                PassengerId: booking.PassengerId,
                PurchaseDate: booking.PurchaseDate,
                PriceAmount: booking.Price.Amount,
                Status: booking.Status
            );
        }
    }
}

