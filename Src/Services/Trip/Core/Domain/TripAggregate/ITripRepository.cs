using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TripAggregate
{
    public interface ITripRepository
    {
        Task AddTripAsync(Trip trip, CancellationToken cancellationToken);

        Task UpdateTripAsync(Trip trip, CancellationToken cancellationToken);

        Task DeleteTripAsync(int tripId, CancellationToken cancellationToken);

        Task<Trip> GetTripByIdAsync(Guid tripId, CancellationToken cancellationToken);

        Task<IEnumerable<Trip>> GetAllTripsAsync(CancellationToken cancellationToken);

        Task<IReadOnlyCollection<Trip>> GetTripsWithPaginationAsync(
                int pageNumber,
                int pageSize,
                CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
