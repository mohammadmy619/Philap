using System.Linq.Expressions;
using BuildingBlocks.Interface;
using Domain.DiscountAggregate;

public interface IDiscountRepository : IRepository
{
    // Discount methods
    Task AddDiscountAsync(Discount discount, CancellationToken cancellationToken);
    Task UpdateDiscountAsync(Discount discount, CancellationToken cancellationToken);
    Task DeleteDiscountAsync(Guid discountId, CancellationToken cancellationToken);
    Task<Discount> GetDiscountByIdAsync(Guid discountId, CancellationToken cancellationToken);
    Task<IEnumerable<Discount>> GetAllDiscountsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Discount>> FindDiscountsAsync(Expression<Func<Discount, bool>> predicate, CancellationToken cancellationToken);
    Task<IEnumerable<Guid>> GetDiscountIdsAsync(IEnumerable<Guid> discountIds, CancellationToken cancellationToken);

    // Related entities methods
    Task<IReadOnlyCollection<Discount>> GetAllDiscountAsync(Guid discountId, CancellationToken cancellationToken);

    // Save changes
    Task SaveChangesAsync(CancellationToken cancellationToken);
}