using FluentValidation;

namespace Application.Validator
{
    public class UpdateTripCommandValidator : AbstractValidator<UpdateTripCommand>
    {
        public UpdateTripCommandValidator()
        {
            RuleFor(x => x.TripId).NotEmpty();
            RuleFor(x => x.LeaderId).NotEmpty();
            RuleFor(x => x.TravelStartDate).LessThan(x => x.TravelEndDate);
            RuleFor(x => x.LocationName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.PriceAmount).GreaterThan(0);
            RuleFor(x => x.PriceCurrency).NotEmpty().Length(3);
        }
    }
}
