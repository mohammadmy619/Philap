using Application.Discounts.GetDiscount;
using Domain.DiscountAggregate;
using Domain.DiscountAggregate.Exceptions;
using MediatR;
using Persistence.Repositories;

namespace Application.Discounts
{
    public class GetDiscountbyIdQueryHandler
        : IRequestHandler<GetDiscountByIdQuery, GetDiscountResponse>
    {
        private readonly IDiscountRepository _discountRepository;

        public GetDiscountbyIdQueryHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<GetDiscountResponse> Handle(
            GetDiscountByIdQuery request,
            CancellationToken cancellationToken)
        {
            // 1. دریافت تخفیف از ریپوزیتوری
            var discount = await _discountRepository.GetDiscountByIdAsync(
                request.DiscountId,
                cancellationToken);

            // 2. مدیریت خطا در صورت عدم یافتن تخفیف
            if (discount is null)
            {
                throw new DiscountNotFoundException(request.DiscountId);
            }

            // 3. مپ کردن Domain Entity به Response DTO
            return new GetDiscountResponse(
                DiscountId: discount.Id,
                Code: discount.Code,
                Type: discount.Type,
                Value: discount.Value,
                ValidFrom: discount.ValidFrom,
                ValidTo: discount.ValidTo,
                MaxUsageCount: discount.MaxUsageCount,
                UsedCount: discount.UsedCount,
                ApplicableTripId: discount.ApplicableTripId,
                ApplicablePassengerId: discount.ApplicablePassengerId,
                IsActive: discount.IsActive,
                CreatedAt: discount.CreatedAt,
                LastUsedAt: discount.LastUsedAt
            );
        }
    }
}