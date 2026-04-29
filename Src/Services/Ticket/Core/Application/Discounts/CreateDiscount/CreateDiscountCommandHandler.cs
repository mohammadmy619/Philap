using Domain.DiscountAggregate;
using MediatR;

public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, Guid>
{
    private readonly IDiscountRepository _discountRepository;

    public CreateDiscountCommandHandler(
        IDiscountRepository discountRepository
       )
    {
        _discountRepository = discountRepository;

    }

    public async Task<Guid> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        // 1. ساخت نمونه جدید (در اینجا Constructor و Guardها اجرا می‌شوند)
        var discount = new Discount(
            code: request.Code,
            type: request.Type,
            value: request.Value,
            validFrom: request.ValidFrom,
            validTo: request.ValidTo,
            maxUsageCount: request.MaxUsageCount,
            applicableTripId: request.ApplicableTripId,
            applicablePassengerId: request.ApplicablePassengerId
        );

        // 2. افزودن به Repository
        await _discountRepository.AddDiscountAsync(discount, cancellationToken);

        // 3. ذخیره تغییرات در دیتابیس
        await _discountRepository.SaveChangesAsync(cancellationToken);

        // 4. بازگشت آیدی موجودیت ساخته شده
        return discount.Id;
    }
}