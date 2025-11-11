using System.Linq.Expressions;
using BuildingBlocks.Interface;
using Domain.AccountingAggregate;

public interface IAccountingRepository : IRepository
{
    // Accounting methods
    Task AddAccountingAsync(Accounting accounting, CancellationToken cancellationToken);
    Task UpdateAccountingAsync(Accounting accounting, CancellationToken cancellationToken);
    Task DeleteAccountingAsync(Guid accountingId, CancellationToken cancellationToken);
    Task<Accounting> GetAccountingByIdAsync(Guid accountingId, CancellationToken cancellationToken);
    Task<IEnumerable<Accounting>> GetAllAccountingsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Accounting>> FindAccountingsAsync(Expression<Func<Accounting, bool>> predicate, CancellationToken cancellationToken);
    Task<IEnumerable<Guid>> GetAccountingIdsAsync(IEnumerable<Guid> accountingIds, CancellationToken cancellationToken);

    // Related entities methods
    Task<IReadOnlyCollection<Accounting>> GetAllAccountingAsync(Guid accountingId, CancellationToken cancellationToken);

    // Save changes
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
