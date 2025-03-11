using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TripAggregate
{
    public interface ITripRepository
    {

        void AddTripAsync(Trip trip);

        Task UpdateTripAsync(Trip trip);

        Task DeleteTripAsync(int tripId);

        Task<Trip> GetTripByIdAsync(int tripId);

        Task<IEnumerable<Trip>> GetAllTripsAsync();

        Task<IEnumerable<Trip>> FindTripsAsync(Func<Trip, bool> predicate);

        Task SaveChangesAsync();
    }
}
