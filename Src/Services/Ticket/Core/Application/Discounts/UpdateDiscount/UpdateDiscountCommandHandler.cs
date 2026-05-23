using BuildingBlocks.Exeptions;
using MediatR;

public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, DiscountUpdatedResponse>
{
    private readonly IDiscountRepository _discountRepository;

    public UpdateDiscountCommandHandler(
        IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<DiscountUpdatedResponse> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        // 1. دریافت موجودیت از دیتابیس
        var discount = await _discountRepository.GetDiscountByIdAsync(request.Id, cancellationToken);

        if (discount == null)
        {
            throw new NotFoundException($"Discount with ID {request.Id} not found.");
        }

        // 2. اعمال تغییرات (متد Update در Entity صدا زده می‌شود و Guardها اجرا می‌شوند)
        discount.Update(
            code: request.Code,
            value: request.Value,
            validFrom: request.ValidFrom,
            validTo: request.ValidTo,
            maxUsageCount: request.MaxUsageCount,
            applicableTripId: request.ApplicableTripId,
            applicablePassengerId: request.ApplicablePassengerId
        );

        // 3. ذخیره تغییرات
        await _discountRepository.SaveChangesAsync(cancellationToken);

        // 4. بازگشت پاسخ
        return new DiscountUpdatedResponse(
            Id: discount.Id,
            Code: discount.Code,
            IsActive: discount.IsActive,
            UpdatedAt: DateTime.UtcNow // یا discount.CreatedAt اگر فیلد آپدیت ندارید
        );
    }
}
