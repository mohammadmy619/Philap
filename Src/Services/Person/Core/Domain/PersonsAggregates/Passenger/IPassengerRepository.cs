using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons.Passenger
{
    public interface IPassengerRepository
    {
        Task AddPassengerAsync(Passenger passenger);

        Task UpdatePassengerAsync(Passenger passenger);
        Task DeletePassengerAsync(Guid passengerId);

        Task<Passenger?> GetPassengerByIdAsync(Guid passengerId);

        Task<IEnumerable<Passenger>> GetAllPassengersAsync();

        Task<IEnumerable<Passenger>> FindPassengersAsync(Func<Passenger, bool> predicate);

        Task SaveChangesAsync();
    }
}
