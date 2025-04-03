using Application.User.CreateUser;
using FluentValidation;


namespace Application.Validator.UserValidation
{
    public class CreateUserValidation : AbstractValidator<CreateUserCommand>   
    {
        public CreateUserValidation()
        {
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
