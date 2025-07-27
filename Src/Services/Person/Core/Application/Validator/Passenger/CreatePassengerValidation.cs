using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Validator.Passenger
{
    public class CreatePassengerValidation : AbstractValidator<CreatePassengerCommand>
    {
        public CreatePassengerValidation()
        {
            // Name validation
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

            // LastName validation
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

            // Email validation
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            // PhoneNumber validation
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^[\+]?[1-9][\d]{0,15}$").WithMessage("Invalid phone number format.");

            // DateOfBirth validation
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.")
                .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Date of birth seems invalid.");

            // Gender validation
            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Invalid gender value.");

            // Street validation
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .Length(5, 200).WithMessage("Street must be between 5 and 200 characters.");

            // City validation
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .Length(2, 100).WithMessage("City must be between 2 and 100 characters.");

            // State validation
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.")
                .Length(2, 100).WithMessage("State must be between 2 and 100 characters.");

            // ZipCode validation
            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("Zip code is required.")
                .Length(4, 10).WithMessage("Zip code must be between 4 and 10 characters.");

            // Nationality validation
            RuleFor(x => x.Nationality)
                .NotEmpty().WithMessage("Nationality is required.")
                .Length(2, 50).WithMessage("Nationality must be between 2 and 50 characters.");

            // PassportNumber validation
            RuleFor(x => x.PassportNumber)
                .NotEmpty().WithMessage("Passport number is required.")
                .Length(6, 20).WithMessage("Passport number must be between 6 and 20 characters.")
                .Matches(@"^[A-Za-z0-9]+$").WithMessage("Passport number can only contain letters and numbers.");

            // FrequentFlyerNumbers validation
            RuleFor(x => x.FrequentFlyerNumbers)
                .NotNull().WithMessage("Frequent flyer numbers list cannot be null.")
                .Must(numbers => numbers.All(n => !string.IsNullOrWhiteSpace(n)))
                .WithMessage("Frequent flyer numbers cannot contain empty values.")
                .When(x => x.FrequentFlyerNumbers != null);

            // Individual frequent flyer number validation
            RuleForEach(x => x.FrequentFlyerNumbers)
                .Length(5, 20).WithMessage("Each frequent flyer number must be between 5 and 20 characters.")
                .Matches(@"^[A-Za-z0-9\-]+$").WithMessage("Frequent flyer numbers can only contain letters, numbers, and hyphens.");
        }
    }
}
