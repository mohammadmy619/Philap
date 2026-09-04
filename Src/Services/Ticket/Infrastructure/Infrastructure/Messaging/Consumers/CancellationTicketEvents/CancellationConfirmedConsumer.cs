using BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents;
using Domain.BookingAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Consumers.CancellationTicketEvents
{
    /// <summary>
    /// این Consumer رویداد تایید نهایی لغو تیکت را دریافت کرده و وضعیت Booking را در دیتابیس به‌روزرسانی می‌کند.
    /// </summary>
    public class CancellationConfirmedConsumer(
        IBookingRepository bookingRepository,
        ILogger<CancellationConfirmedConsumer> logger) : IConsumer<ICancellationConfirmedEvent>
    {
        public async Task Consume(ConsumeContext<ICancellationConfirmedEvent> context)
        {
            var message = context.Message;

            logger.LogInformation(
                "Processing CancellationConfirmedEvent | TicketId: {TicketId}, PassengerId: {PassengerId}, Reason: {Reason}",
                message.TicketId, // فرض بر این است که TicketId در IBaseTicketEvent تعریف شده است
                message.PassengerId,
                message.Reason);

            // ۱. دریافت موجودیت از دیتابیس
            var booking = await bookingRepository.GetBookingByIdAsync(message.TicketId, context.CancellationToken);

            // ۲. مدیریت حالت Idempotent (اگر رکورد پیدا نشد یا قبلاً کنسل شده، بدون خطا خارج می‌شویم)
            if (booking == null)
            {
                logger.LogWarning("Booking not found for cancellation confirmation | TicketId: {TicketId}", message.TicketId);
                return;
            }

            // ۳. اعمال منطق دامین (Domain Logic) برای تغییر وضعیت
            booking.ChangeStatus(BookingStatus.Cancelled);

            // ۴. ذخیره تغییرات در دیتابیس
            await bookingRepository.UpdateBookingAsync(booking, context.CancellationToken);
            await bookingRepository.SaveChangesAsync(context.CancellationToken);

            // ۵. ثبت لاگ موفقیت
            logger.LogInformation(
                "Booking {BookingId} status successfully updated to Cancelled | TicketId: {TicketId}",
                booking.Id,
                message.TicketId);
        }
    }
}