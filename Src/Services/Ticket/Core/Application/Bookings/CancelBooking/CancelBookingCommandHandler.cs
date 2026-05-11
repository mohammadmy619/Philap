using Domain.BookingAggregate;
using MediatR;

namespace Application.Ticketing
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, CancelBookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public CancelBookingCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<CancelBookingResponse> Handle(CancelBookingCommand command, CancellationToken cancellationToken)
        {
            // 1. بارگذاری رزرو از دیتابیس
            var booking = await _bookingRepository.GetBookingByIdAsync(command.BookingId, cancellationToken)
                ?? throw new InvalidOperationException($"Booking with Id {command.BookingId} not found");

            // 2. ذخیره وضعیت قبلی (برای نمایش در پاسخ)
            var previousStatus = booking.Status;

            // 3. فراخوانی متد دامنه برای لغو رزرو
            // این متد بررسی می‌کند که فقط رزروهای Confirmed قابل لغو هستند
            booking.Cancel();

            // 4. ذخیره تغییرات (شامل ثبت Domain Eventها)
            await _bookingRepository.SaveChangesAsync(cancellationToken);

            // 5. بازگرداندن پاسخ
            return new CancelBookingResponse(
                BookingId: booking.Id,
                PreviousStatus: previousStatus,
                CurrentStatus: booking.Status,
                CancelledAt: DateTime.Now
            );
        }
    }
}