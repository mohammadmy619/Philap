using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Persons.Passenger;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class PassengerRepository(PersonDbContext _context) : IPassengerRepository
    {

      
        public async Task AddPassengerAsync(Passenger passenger, CancellationToken cancellationToken)
        {
            await _context.Passengers.AddAsync(passenger, cancellationToken);
        }

        public async Task UpdatePassengerAsync(Passenger passenger, CancellationToken cancellationToken)
        {
            _context.Passengers.Update(passenger);
            await Task.CompletedTask; // SaveChanges در SaveChangesAsync انجام می‌شود
        }

        public async Task DeletePassengerAsync(Guid passengerId, CancellationToken cancellationToken)
        {
            var passenger = await _context.Passengers
                .FindAsync(new object[] { passengerId }, cancellationToken: cancellationToken);

            if (passenger != null)
            {
                _context.Passengers.Remove(passenger);
            }
        }

        public async Task<Passenger?> GetPassengerByIdAsync(Guid passengerId, CancellationToken cancellationToken)
        {
            return await _context.Passengers
                .Include(p => p.Address) // فرض می‌کنیم Address navigation property هست
                .FirstOrDefaultAsync(p => p.Id == passengerId, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Passenger>> GetAllPassengersAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Passengers.AsQueryable()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Passenger>> FindPassengersAsync(Expression<Func<Passenger, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Passengers.AsQueryable()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
