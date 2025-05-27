using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Persons.Leader;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositores
{
    public class LeaderRepository(PersonDbContext _context) : ILeaderRepository
    {

        public async Task AddLeaderAsync(Leader leader, CancellationToken cancellationToken)
        {
            await _context.Leaders.AddAsync(leader, cancellationToken);
        }

        public async Task UpdateLeaderAsync(Leader leader, CancellationToken cancellationToken)
        {
            _context.Leaders.Update(leader);
            await Task.CompletedTask; // SaveChanges در SaveChangesAsync صورت می‌گیرد
        }

        public async Task DeleteLeaderAsync(Guid leaderId, CancellationToken cancellationToken)
        {
            var leader = await _context.Leaders.FindAsync(new object[] { leaderId }, cancellationToken: cancellationToken);
            if (leader != null)
            {
                _context.Leaders.Remove(leader);
            }
        }

        public async Task<Leader?> GetLeaderByIdAsync(Guid leaderId, CancellationToken cancellationToken)
        {
            return await _context.Leaders
                .Include(l => l.Address) // اگر لازم باشد
                .FirstOrDefaultAsync(l => l.Id == leaderId, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Leader>> GetLeaders(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Leaders.AsQueryable()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Leader>> FindLeadersAsync(Expression<Func<Leader, bool>> predicate  , CancellationToken cancellationToken)
        {
            return await _context.Leaders.AsQueryable().Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
