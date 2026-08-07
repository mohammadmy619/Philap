using Domain.BookingAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly TicketingDbContext _dbContext;

        public BookingRepository(TicketingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddBookingAsync(Booking booking, CancellationToken cancellationToken)
        {
            if (booking is null)
                throw new ArgumentNullException(nameof(booking));

            await _dbContext.Booking.AddAsync(booking, cancellationToken);
        }

        public async Task DeleteBookingAsync(Guid bookingId, CancellationToken cancellationToken)
        {
            var booking = await _dbContext.Booking
                .FirstOrDefaultAsync(x => x.Id == bookingId, cancellationToken);

            if (booking is null)
                return;

            _dbContext.Booking.Remove(booking);
        }

        public async Task<IEnumerable<Booking>> FindBookingsAsync(
            Expression<Func<Booking, bool>> predicate,
            CancellationToken cancellationToken)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return await _dbContext.Booking
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<Booking>> GetAllBookingAsync(
            Guid bookingId,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Booking.AsNoTracking();

            if (bookingId != Guid.Empty)
            {
                query = query.Where(x => x.Id == bookingId);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Booking
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Booking> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken)
        {
            return await _dbContext.Booking
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == bookingId, cancellationToken);
        }

        public async Task<IEnumerable<Guid>> GetBookingIdsAsync(
            IEnumerable<Guid> bookingIds,
            CancellationToken cancellationToken)
        {
            if (bookingIds is null)
                throw new ArgumentNullException(nameof(bookingIds));

            var ids = bookingIds as Guid[] ?? bookingIds.ToArray();

            if (ids.Length == 0)
                return Enumerable.Empty<Guid>();

            return await _dbContext.Booking
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateBookingAsync(Booking booking, CancellationToken cancellationToken)
        {
            if (booking is null)
                throw new ArgumentNullException(nameof(booking));

            _dbContext.Booking.Update(booking);

            return Task.CompletedTask;
        }
    }
}