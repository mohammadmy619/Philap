using Domain.AccountingAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class AccountingRepository : IAccountingRepository
    {
        private readonly TicketingDbContext _dbContext;

        public AccountingRepository(TicketingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAccountingAsync(Accounting accounting, CancellationToken cancellationToken)
        {
            if (accounting is null)
                throw new ArgumentNullException(nameof(accounting));

            await _dbContext.Accounting.AddAsync(accounting, cancellationToken);
        }

        public async Task DeleteAccountingAsync(Guid accountingId, CancellationToken cancellationToken)
        {
            var accounting = await _dbContext.Accounting
                .FirstOrDefaultAsync(x => x.Id == accountingId, cancellationToken);

            if (accounting is null)
                return;

            _dbContext.Accounting.Remove(accounting);
        }

        public async Task<IEnumerable<Accounting>> FindAccountingsAsync(
            Expression<Func<Accounting, bool>> predicate,
            CancellationToken cancellationToken)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return await _dbContext.Accounting
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Accounting> GetAccountingByIdAsync(Guid accountingId, CancellationToken cancellationToken)
        {
            return await _dbContext.Accounting
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == accountingId, cancellationToken);
        }

        public async Task<IEnumerable<Guid>> GetAccountingIdsAsync(
            IEnumerable<Guid> accountingIds,
            CancellationToken cancellationToken)
        {
            if (accountingIds is null)
                throw new ArgumentNullException(nameof(accountingIds));

            var ids = accountingIds as Guid[] ?? accountingIds.ToArray();

            if (ids.Length == 0)
                return Enumerable.Empty<Guid>();

            return await _dbContext.Accounting
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<Accounting>> GetAllAccountingAsync(
            Guid accountingId,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Accounting.AsNoTracking();

            if (accountingId != Guid.Empty)
            {
                query = query.Where(x => x.Id == accountingId);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Accounting>> GetAllAccountingsAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Accounting
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAccountingAsync(Accounting accounting, CancellationToken cancellationToken)
        {
            if (accounting is null)
                throw new ArgumentNullException(nameof(accounting));

            _dbContext.Accounting.Update(accounting);

            return Task.CompletedTask;
        }
    }
}