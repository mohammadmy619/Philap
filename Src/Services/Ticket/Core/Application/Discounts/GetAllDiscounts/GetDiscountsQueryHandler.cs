using Application.Discounts.GetAllDiscounts;
using Application.Discounts.GetDiscount;
using Domain.DiscountAggregate;
using MediatR;
using Persistence.Repositories;

namespace Application.Discounts
{
    public class GetDiscountsQueryHandler
        : IRequestHandler<GetDiscountsQuery, GetDiscountsResponse>
    {
        private readonly IDiscountRepository _discountRepository;

        public GetDiscountsQueryHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<GetDiscountsResponse> Handle(
            GetDiscountsQuery request,
            CancellationToken cancellationToken)
        {
            // اعتبارسنجی مقادیر ورودی
            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;
            pageSize = pageSize > 100 ? 100 : pageSize; // محدود کردن حداکثر PageSize

            // دریافت تمام تخفیف‌ها (در پروژه‌های بزرگ بهتر است Pagination در Repository اعمال شود)
            var allDiscounts = await _discountRepository.GetAllDiscountsAsync(cancellationToken);

            var totalCount = allDiscounts.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var discounts = allDiscounts
                .OrderByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new GetDiscountResponse(
                    DiscountId: d.Id,
                    Code: d.Code,
                    Type: d.Type,
                    Value: d.Value,
                    ValidFrom: d.ValidFrom,
                    ValidTo: d.ValidTo,
                    MaxUsageCount: d.MaxUsageCount,
                    UsedCount: d.UsedCount,
                    ApplicableTripId: d.ApplicableTripId,
                    ApplicablePassengerId: d.ApplicablePassengerId,
                    IsActive: d.IsActive,
                    CreatedAt: d.CreatedAt,
                    LastUsedAt: d.LastUsedAt
                ))
                .ToList();

            return new GetDiscountsResponse(
                Discounts: discounts,
                TotalCount: totalCount,
                Page: page,
                PageSize: pageSize,
                TotalPages: totalPages
            );
        }
    }
}