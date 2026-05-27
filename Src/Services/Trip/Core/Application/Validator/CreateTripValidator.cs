using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Trips.CreateTrip;
using FluentValidation;

namespace Application.Validator
{
    public class CreateTripCommandValidator : AbstractValidator<CreateTripCommand>
    {
        public CreateTripCommandValidator()
        {
            //  RuleFor(x => x.LeaderId)
            //.NotEmpty()
            //.WithMessage("LeaderId is required.");
            RuleFor(x => x.LeaderId)
      .NotEmpty().WithMessage("LeaderId is required.")
      .Must(z => Guid.TryParse(z.ToString(), out _)).WithMessage("LeaderId must be a valid GUID format.");



            RuleFor(x => x.TravelStartDate)
                .GreaterThan(DateTime.Now)
                .WithMessage("Travel start date must be in the future.");

            RuleFor(x => x.TravelEndDate)
                .GreaterThan(x => x.TravelStartDate)
                .WithMessage("Travel end date must be after the start date.");

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
