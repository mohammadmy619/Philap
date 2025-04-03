using Application.Role.CreateRole;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validator.RoleValidation
{
   public class CreateRoleValidation: AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidation()
        {
            RuleFor(x => x.Name)
          .NotEmpty().WithMessage("Name is required.")
          .Length(3, 50).WithMessage("Name must be between 3 and 20 characters.");

          
        }
    }
}
