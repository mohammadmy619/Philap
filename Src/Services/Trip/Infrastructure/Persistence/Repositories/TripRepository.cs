using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.TripAggregate;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class TripRepository(TripDbContext _TripDbContext) : ITripRepository
    {
        public async Task AddTripAsync(Trip trip, CancellationToken cancellationToken)
        {
            if (trip is null)
                throw new ArgumentNullException(nameof(trip));

            await _TripDbContext.Trip.AddAsync(trip, cancellationToken);
        }

        public async Task DeleteTripAsync(Guid tripId, CancellationToken cancellationToken)
        {
            var trip = await _TripDbContext.Trip
                .FirstOrDefaultAsync(t => t.Id == tripId, cancellationToken);

            if (trip is not null)
            {
                _TripDbContext.Trip.Remove(trip);
                await _TripDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<Trip>> GetAllTripsAsync(CancellationToken cancellationToken)
        {
            return await _TripDbContext.Trip.ToListAsync(cancellationToken);
        }

        public async Task<Trip> GetTripByIdAsync(Guid tripId, CancellationToken cancellationToken)
        {
            return await _TripDbContext.Trip
                .FirstOrDefaultAsync(t => t.Id == tripId, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Trip>> GetTripsWithPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Page number and page size must be greater than zero.");

            var trips = await _TripDbContext.Trip
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return trips.AsReadOnly();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _TripDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTripAsync(Trip trip, CancellationToken cancellationToken)
        {
            if (trip is null)
                throw new ArgumentNullException(nameof(trip));

            _TripDbContext.Trip.Update(trip);
        }
    }
}
