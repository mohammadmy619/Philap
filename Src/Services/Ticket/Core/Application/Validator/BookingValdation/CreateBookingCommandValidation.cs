using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Ticketing;
using FluentValidation;

namespace Application.Validator.BookingValdation
{
    public class CreateBookingCommandValidation : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidation()
        {
            // اعتبارسنجی TicketId
            RuleFor(x => x.TicketId)
                .NotEmpty().WithMessage("TicketId is required.");

            // اعتبارسنجی TripId
            RuleFor(x => x.TripId)
                .NotEmpty().WithMessage("TripId is required.");

            // اعتبارسنجی PassengerId
            RuleFor(x => x.PassengerId)
                .NotEmpty().WithMessage("PassengerId is required.");

            // اعتبارسنجی تاریخ خرید (نباید در آینده باشد)
            RuleFor(x => x.PurchaseDate)
                .NotEmpty().WithMessage("PurchaseDate is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("PurchaseDate cannot be in the future.");

            // اعتبارسنجی مبلغ قیمت (باید مثبت باشد)
            RuleFor(x => x.PriceAmount)
                .GreaterThan(0).WithMessage("PriceAmount must be greater than 0.");

            // اعتبارسنجی واحد پول
            RuleFor(x => x.PriceCurrency)
                .NotEmpty().WithMessage("PriceCurrency is required.")
                .Length(3, 10).WithMessage("PriceCurrency must be between 3 and 10 characters.");

            // اعتبارسنجی توضیحات قیمت (اختیاری)
            RuleFor(x => x.PriceDescription)
                .MaximumLength(200).WithMessage("PriceDescription cannot exceed 200 characters.")
                .When(x => !string.IsNullOrEmpty(x.PriceDescription));
        }
    }
}
