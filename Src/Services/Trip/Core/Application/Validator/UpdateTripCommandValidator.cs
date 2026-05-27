using FluentValidation;

namespace Application.Validator
{
    public class UpdateTripCommandValidator : AbstractValidator<UpdateTripCommand>
    {
        public UpdateTripCommandValidator()
        {
            RuleFor(x => x.TripId)
              .NotEmpty()
              .WithMessage("TripId is required.");

            RuleFor(x => x.LeaderId)
                .NotEmpty()
                .WithMessage("LeaderId is required.");

            RuleFor(x => x.TravelStartDate)
                .LessThan(x => x.TravelEndDate)
                .WithMessage("Travel start date must be before the end date.");

            RuleFor(x => x.LocationName)
                .NotEmpty()
                .WithMessage("Location name is required.")
                .MaximumLength(200)
                .WithMessage("Location name must not exceed 200 characters.");

            RuleFor(x => x.PriceAmount)
                .GreaterThan(0)
                .WithMessage("Price amount must be greater than zero.");

            RuleFor(x => x.PriceCurrency)
                .NotEmpty()
                .WithMessage("Price currency is required.")
                .Length(3)
                .WithMessage("Price currency must be exactly 3 characters.");
        }
    }
}
