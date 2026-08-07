using Domain.DiscountAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly TicketingDbContext _dbContext;

        public DiscountRepository(TicketingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Discount Methods

        public async Task AddDiscountAsync(Discount discount, CancellationToken cancellationToken)
        {
            if (discount is null)
                throw new ArgumentNullException(nameof(discount));

            await _dbContext.Discount.AddAsync(discount, cancellationToken);
        }

        public async Task DeleteDiscountAsync(Guid discountId, CancellationToken cancellationToken)
        {
            var discount = await _dbContext.Discount
                .FirstOrDefaultAsync(x => x.Id == discountId, cancellationToken);

            if (discount is null)
                return;

            _dbContext.Discount.Remove(discount);
        }

        public async Task<IEnumerable<Discount>> FindDiscountsAsync(
            Expression<Func<Discount, bool>> predicate,
            CancellationToken cancellationToken)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return await _dbContext.Discount
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<Discount>> GetAllDiscountAsync(
            Guid discountId,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Discount.AsNoTracking();

            if (discountId != Guid.Empty)
            {
                query = query.Where(x => x.Id == discountId);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Discount>> GetAllDiscountsAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Discount
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Discount> GetDiscountByIdAsync(Guid discountId, CancellationToken cancellationToken)
        {
            return await _dbContext.Discount
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == discountId, cancellationToken);
        }

        public async Task<IEnumerable<Guid>> GetDiscountIdsAsync(
            IEnumerable<Guid> discountIds,
            CancellationToken cancellationToken)
        {
            if (discountIds is null)
                throw new ArgumentNullException(nameof(discountIds));

            var ids = discountIds as Guid[] ?? discountIds.ToArray();

            if (ids.Length == 0)
                return Enumerable.Empty<Guid>();

            return await _dbContext.Discount
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
        }

        public Task UpdateDiscountAsync(Discount discount, CancellationToken cancellationToken)
        {
            if (discount is null)
                throw new ArgumentNullException(nameof(discount));

            _dbContext.Discount.Update(discount);

            return Task.CompletedTask;
        }

        #endregion

        #region DiscountUsage Methods

        public async Task AddDiscountUsageAsync(DiscountUsage usage, CancellationToken cancellationToken)
        {
            if (usage is null)
                throw new ArgumentNullException(nameof(usage));

            await _dbContext.DiscountUsage.AddAsync(usage, cancellationToken);
        }

        public async Task<DiscountUsage?> GetUsageByIdAsync(Guid usageId, CancellationToken cancellationToken)
        {
            return await _dbContext.DiscountUsage
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == usageId, cancellationToken);
        }

        public async Task<IReadOnlyCollection<DiscountUsage>> GetUsagesByBookingIdAsync(
            Guid bookingId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.DiscountUsage
                .AsNoTracking()
                .Where(u => u.BookingId == bookingId)
                .OrderByDescending(u => u.UsedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<DiscountUsage>> GetUsagesByDiscountIdAsync(
            Guid discountId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.DiscountUsage
                .AsNoTracking()
                .Where(u => u.DiscountId == discountId)
                .OrderByDescending(u => u.UsedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<DiscountUsage>> GetUsagesByPassengerIdAsync(
            Guid passengerId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.DiscountUsage
                .AsNoTracking()
                .Where(u => u.PassengerId == passengerId)
                .OrderByDescending(u => u.UsedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasUsageForBookingAsync(
            Guid discountId,
            Guid bookingId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.DiscountUsage
                .AsNoTracking()
                .AnyAsync(u => u.DiscountId == discountId && u.BookingId == bookingId, cancellationToken);
        }

        #endregion

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}