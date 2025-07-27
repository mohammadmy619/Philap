using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Application.Leader.UpdateLeader;
using FluentValidation;

namespace Application.Validator.Leader
{
    public class UpdateLeaderValidation : AbstractValidator<UpdateLeaderCommand>
    {
        public UpdateLeaderValidation()
        {
            // LeaderId validation
            RuleFor(x => x.LeaderId)
                .NotEmpty().WithMessage("Leader ID is required.");

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

            // Nationality validation
            RuleFor(x => x.Nationality)
                .NotEmpty().WithMessage("Nationality is required.")
                .Length(2, 50).WithMessage("Nationality must be between 2 and 50 characters.");

            // Title validation
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(2, 100).WithMessage("Title must be between 2 and 100 characters.");

            // Department validation
            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required.")
                .Length(2, 100).WithMessage("Department must be between 2 and 100 characters.");

            // Bio validation
            RuleFor(x => x.Bio)
                .MaximumLength(500).WithMessage("Bio cannot exceed 500 characters.");

            // Address validations
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .Length(5, 200).WithMessage("Street must be between 5 and 200 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .Length(2, 100).WithMessage("City must be between 2 and 100 characters.");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.")
                .Length(2, 100).WithMessage("State must be between 2 and 100 characters.");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("Zip code is required.")
                .Length(4, 10).WithMessage("Zip code must be between 4 and 10 characters.");

            // DateOfBirth validation
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

            // JoiningDate validation
            RuleFor(x => x.JoiningDate)
                .NotEmpty().WithMessage("Joining date is required.")
                .GreaterThanOrEqualTo(DateTime.Today.AddYears(-50)).WithMessage("Joining date seems invalid.");

            // Skills validation
            RuleFor(x => x.Skills)
                .NotNull().WithMessage("Skills list cannot be null.")
                .Must(skills => skills.All(s => !string.IsNullOrWhiteSpace(s)))
                .WithMessage("Skills cannot contain empty values.")
                .When(x => x.Skills != null);

            // Gender validation
            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Invalid gender value.");
        }
    }
}
