using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;
using Domain.BookingAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Consumers
{
    public class CancelUpdateTicketConsumer(
        IBookingRepository bookingRepository,
        ILogger<CancelUpdateTicketConsumer> logger) : IConsumer<ICancelUpdateTripEvent>
    {
        public async Task Consume(ConsumeContext<ICancelUpdateTripEvent> context)
        {
            var message = context.Message;

            logger.LogWarning(
                "Processing CancelUpdateTripEvent (Rollback/Failure) | TicketId: {TicketId}, TripId: {TripId}, Reason: {Reason}",
                message.TicketId,
                message.TripId,
                message.Reason);

            var booking = await bookingRepository.GetBookingByIdAsync(message.TicketId, context.CancellationToken);

            if (booking == null)
            {
                logger.LogWarning("Booking not found for update cancellation | TicketId: {TicketId}", message.TicketId);
                return;
            }

 
            booking.ChangeStatus(BookingStatus.Cancelled);

            await bookingRepository.UpdateBookingAsync(booking, context.CancellationToken);
            await bookingRepository.SaveChangesAsync(context.CancellationToken);

            logger.LogInformation(
                "Booking {BookingId} status updated after failed update | TicketId: {TicketId}",
                booking.Id,
                message.TicketId);
        }
    }
}
