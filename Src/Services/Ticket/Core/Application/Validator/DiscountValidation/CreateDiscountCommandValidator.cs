using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DiscountAggregate;
using FluentValidation;

namespace Application.Validator.DiscountValidation
{

    public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
    {
        public CreateDiscountCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Discount code is required.")
                .MaximumLength(50);

            RuleFor(x => x.Value)
                .GreaterThanOrEqualTo(0).WithMessage("Value cannot be negative.");

            RuleFor(x => x.Type)
                .IsInEnum();

            // اعتبارسنجی خاص برای درصد
            RuleFor(x => x.Value)
                .LessThanOrEqualTo(100)
                .When(x => x.Type == DiscountType.Percentage)
                .WithMessage("Percentage discount cannot exceed 100%.");

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
}
