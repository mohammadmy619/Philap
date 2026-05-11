using FluentValidation;

public class UpdateDiscountCommandValidator : AbstractValidator<UpdateDiscountCommand>
{
    public UpdateDiscountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Discount ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Discount code is required.")
            .MaximumLength(50);

        RuleFor(x => x.Value)
            .GreaterThanOrEqualTo(0).WithMessage("Value cannot be negative.");

        // اعتبارسنجی بازه زمانی
        RuleFor(x => x.ValidTo)
            .GreaterThanOrEqualTo(x => x.ValidFrom)
            .When(x => x.ValidFrom.HasValue && x.ValidTo.HasValue)
            .WithMessage("ValidTo cannot be before ValidFrom.");

        RuleFor(x => x.MaxUsageCount)
            .GreaterThan(0)
            .When(x => x.MaxUsageCount.HasValue)
            .WithMessage("Max usage count must be greater than zero.");
    }
}