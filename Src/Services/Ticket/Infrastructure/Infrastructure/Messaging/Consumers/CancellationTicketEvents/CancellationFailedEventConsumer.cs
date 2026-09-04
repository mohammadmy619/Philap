using BuildingBlocks.Messaging.TicketEvents.CancelTicketEvents;
using Domain.BookingAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Consumers.CancellationTicketEvents
{

    public class CancellationFailedEventConsumer(
        IBookingRepository bookingRepository,
        ILogger<CancellationFailedEventConsumer> logger) : IConsumer<ITripCancellationFailedEvent>
    {
        public async Task Consume(ConsumeContext<ITripCancellationFailedEvent> context)
        {
            var message = context.Message;

            logger.LogInformation(
                "Processing CancellationFailedEvent (Compensation) | TicketId: {TicketId}, PassengerId: {PassengerId}, Reason: {Reason}",
                message.TicketId,
                message.PassengerId,
                message.Reason);

            var booking = await bookingRepository.GetBookingByIdAsync(message.TicketId, context.CancellationToken);

            if (booking == null)
            {
                logger.LogWarning("Booking not found for cancellation failure handling | TicketId: {TicketId}", message.TicketId);
                return;
            }

            
            booking.ChangeStatus(BookingStatus.Confirmed);

            // ۴. ذخیره تغییرات در دیتابیس
            await bookingRepository.UpdateBookingAsync(booking, context.CancellationToken);
            await bookingRepository.SaveChangesAsync(context.CancellationToken);

            // ۵. ثبت لاگ موفقیت
            logger.LogInformation(
                "Booking {BookingId} status successfully reverted to Active due to cancellation failure | TicketId: {TicketId}",
                booking.Id,
                message.TicketId);
        }
    }
}