using BuildingBlocks.Messaging.TicketEvents;
using BuildingBlocks.Messaging.TicketEvents.UpdateBookingEvents;
using Domain.BookingAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Consumers.UpdateTicketEvents
{
    public class AcceptUpdateTicketConsumer(
        IBookingRepository bookingRepository,
        ILogger<AcceptUpdateTicketConsumer> _logger) : IConsumer<IAcceptUpdateTicketEvent>
    {
        public async Task Consume(ConsumeContext<IAcceptUpdateTicketEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "Processing AcceptUpdateTicketEvent | TicketId: {TicketId}, TripId: {TripId}, PassengerId: {PassengerId}",
                message.TicketId,
                message.TripId,
                message.PassengerId);

            var booking = await bookingRepository.GetBookingByIdAsync(message.TicketId, context.CancellationToken);

            if (booking == null)
            {
                _logger.LogWarning("Booking not found for TicketId: {TicketId}", message.TicketId);
                return;
            }

            booking.ChangeStatus(BookingStatus.Confirmed);

            

            await bookingRepository.UpdateBookingAsync(booking, context.CancellationToken);
            await bookingRepository.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation(
                "Booking {BookingId} successfully updated and confirmed | TicketId: {TicketId}",
                booking.Id,
                message.TicketId);
        }
    }
}
