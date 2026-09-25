using BuildingBlocks.Messaging.TicketEvents;
using Domain.BookingAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Consumers.AddTicketEvents
{
    public class AcceptTicketConsumer(IBookingRepository bookingRepository, ILogger<AcceptTicketConsumer> _logger) : IConsumer<IAcceptTicketEvent>
    {
        public async Task Consume(ConsumeContext<IAcceptTicketEvent> context)
        {

            var booking = await bookingRepository.GetBookingByIdAsync(context.Message.TicketId, context.CancellationToken);

            if (booking == null)
            {
                 _logger.LogWarning("Booking not found for TicketId: {TicketId}", context.Message.TicketId);
                return;
            }

            booking.ChangeStatus(BookingStatus.Confirmed);

             await  bookingRepository.UpdateBookingAsync(booking, context.CancellationToken);

            await bookingRepository.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("Booking {BookingId} successfully confirmed for TicketId: {TicketId}", booking.Id, context.Message.TicketId);


        }
    }
}