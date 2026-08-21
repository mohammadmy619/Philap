using BuildingBlocks.Messaging.TicketEvents;
using Domain.BookingAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Consumers
{
    // استفاده از Primary Constructor مشابه کد AcceptTicketConsumer شما
    public class CancelTripConsumer(IBookingRepository bookingRepository, ILogger<CancelTripConsumer> _logger) : IConsumer<ICancelTripEvent>
    {
        public async Task Consume(ConsumeContext<ICancelTripEvent> context)
        {
            var booking = await bookingRepository.GetBookingByIdAsync(context.Message.TicketId, context.CancellationToken);

            if (booking == null)
            {
                _logger.LogWarning("Booking not found for cancellation. TicketId: {TicketId}", context.Message.TicketId);
                return;
            }

            booking.ChangeStatus(BookingStatus.Cancelled);

            await bookingRepository.UpdateBookingAsync(booking, context.CancellationToken);
            await bookingRepository.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("Booking {BookingId} successfully cancelled for TicketId: {TicketId}", booking.Id, context.Message.TicketId);
        }
    }
}