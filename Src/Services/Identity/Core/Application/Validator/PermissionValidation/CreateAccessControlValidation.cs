using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Permission.CreateAccessControl;
using FluentValidation;

namespace Application.Validator.PermissionValidation
{
    public class CreateAccessControlValidation : AbstractValidator<CreateAccessControlCommand>
    {
        public CreateAccessControlValidation()
        {
            RuleFor(x => x.PermissionId)
                .NotEmpty().WithMessage("PermissionId is required.");

            RuleFor(x => x.Resource)
          .NotEmpty().WithMessage("Resource is required.")
          .Length(3, 50).WithMessage("Name must be between 3 and 50 characters.");
            
            RuleFor(x => x.Action)
          .NotEmpty().WithMessage("Action is required.")
          .Length(3, 150).WithMessage("Action must be between 3 and 150 characters.");
        }
    }
}
