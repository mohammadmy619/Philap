using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons.Passenger
{
    public interface IPassengerRepository
    {
        Task AddPassengerAsync(Passenger passenger, CancellationToken cancellationToken);

        Task UpdatePassengerAsync(Passenger passenger, CancellationToken cancellationToken);

        Task DeletePassengerAsync(Guid passengerId, CancellationToken cancellationToken);

        Task<Passenger?> GetPassengerByIdAsync(Guid passengerId, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<Passenger>> GetAllPassengersAsync(int page, int pageSize, CancellationToken cancellationToken);

        Task<IEnumerable<Passenger>> FindPassengersAsync(Expression<Func<Passenger, bool>> predicate, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
