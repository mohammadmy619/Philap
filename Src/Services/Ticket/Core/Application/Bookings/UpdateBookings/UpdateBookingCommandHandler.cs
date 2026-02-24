using Application.Bookings.UpdateBookings;
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
            // 1. بارگذاری رزرو از دیتابیس
            var booking = await _bookingRepository.GetBookingByIdAsync(command.BookingId, cancellationToken)
                ?? throw new InvalidOperationException($"Booking with Id {command.BookingId} not found");
        
            // 2. اعتبارسنجی بیزینسی (اختیاری: مثلاً رزروهای کنسل شده قابل آپدیت نباشند)
            if (booking.Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException("Cancelled bookings cannot be updated");
            }

            // 3. فراخوانی متد دامنه برای اعمال تغییرات
            var newPrice = new Money(command.PriceAmount, command.PriceCurrency);
            booking.Update(
                command.TripId,
                command.PassengerId,
                command.PurchaseDate,
                newPrice
            );

            // 4. ذخیره تغییرات
            await _bookingRepository.SaveChangesAsync(cancellationToken);

            // 5. بازگرداندن پاسخ
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

