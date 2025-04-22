using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Permission.CreatePermission;
using Application.Permission.UpdatePermission;
using FluentValidation;

namespace Application.Validator.PermissionValidation
{
    public class UpdatePermissionValidation : AbstractValidator<Permission.UpdatePermission.UpdatePermissionCommand>
    {
        public UpdatePermissionValidation()
        {
            RuleFor(x => x.Id)
             .NotNull().WithMessage("Id is required.");


            RuleFor(x => x.Name)
          .NotEmpty().WithMessage("Name is required.")
          .Length(3, 50).WithMessage("Name must be between 3 and 20 characters.");
        }
    }
}
