using Application.Discounts.GetDiscountUsagesPaged;
using MediatR;

namespace Application.Discounts.GetDiscountUsages
{
    public class GetDiscountUsagesPagedQueryHandler
        : IRequestHandler<GetDiscountUsagesPagedQuery, GetDiscountUsagesPagedResponse>
    {
        private readonly IDiscountRepository _discountRepository;

        public GetDiscountUsagesPagedQueryHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<GetDiscountUsagesPagedResponse> Handle(
            GetDiscountUsagesPagedQuery request,
            CancellationToken cancellationToken)
        {
            // 1. اعتبارسنجی مقادیر ورودی
            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            // 2. دریافت تمام استفاده‌های این تخفیف
            var allUsages = await _discountRepository.GetUsagesByDiscountIdAsync(
                request.DiscountId,
                cancellationToken);

            // 3. محاسبه اطلاعات Pagination
            var totalCount = allUsages.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // 4. اعمال Skip و Take برای صفحه مورد نظر
            var usages = allUsages
                .OrderByDescending(u => u.UsedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new GetDiscountUsageResponse(
                    UsageId: u.Id,
                    DiscountId: u.DiscountId,
                    BookingId: u.BookingId,
                    PassengerId: u.PassengerId,
                    TripId: u.TripId,
                    AppliedAmount: u.AppliedAmount,
                    UsedAt: u.UsedAt
                ))
                .ToList();

            // 5. بازگشت Response
            return new GetDiscountUsagesPagedResponse(
                Usages: usages,
                TotalCount: totalCount,
                Page: page,
                PageSize: pageSize,
                TotalPages: totalPages
            );
        }
    }
}