using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Interface;

namespace Domain.BookingAggregate
{
    public interface IBookingRepository:IRepository
    {

        // Booking methods
        Task AddBookingAsync(Booking booking, CancellationToken cancellationToken);
        Task UpdateBookingAsync(Booking booking, CancellationToken cancellationToken);
        Task DeleteBookingAsync(Guid bookingId, CancellationToken cancellationToken);
        Task<Booking> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken);
        Task<IEnumerable<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Booking>> FindBookingsAsync(Func<Booking, bool> predicate, CancellationToken cancellationToken);
        Task<IEnumerable<Guid>> GetBookingIdsAsync(IEnumerable<Guid> bookingIds, CancellationToken cancellationToken);

        // Related entities methods (similar to AccessControl in your example)
        // For example, if you have BookingDetails or Tickets related to bookings:
        Task<IReadOnlyCollection<Booking>> GetAllBookingAsync(Guid bookingId, CancellationToken cancellationToken);

        // Save changes
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
