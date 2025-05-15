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
            RuleFor(x => x.LeaderId).NotEmpty();
            RuleFor(x => x.TravelStartDate).GreaterThan(DateTime.Now);
            RuleFor(x => x.TravelEndDate).GreaterThan(x => x.TravelStartDate);
            RuleFor(x => x.LocationName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.PriceAmount).GreaterThan(0);
            RuleFor(x => x.PriceCurrency).NotEmpty().Length(3);
        }
    }
}
