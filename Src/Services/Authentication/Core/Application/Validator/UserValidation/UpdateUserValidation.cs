using Application.User.CreateUser;
using Application.User.UpdateUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validator.UserValidation
{
  public class UpdateUserValidation : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidation()
        {
            RuleFor(x => x.UserId)
           .NotEmpty();

            RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 20).WithMessage("Username must be between 3 and 20 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(3).WithMessage("Password must be at least 6 characters long.");
        }
    }
    
}
