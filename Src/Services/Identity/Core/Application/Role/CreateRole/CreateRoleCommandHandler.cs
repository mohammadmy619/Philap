using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Domain_Services;
using Domain.RoleAgregate;
using Domain.RoleAgregate.Exception;
using MediatR;

namespace Application.Role.CreateRole
{
    public class CreateRoleCommandHandler(IRoleRepository _roleRepository, IPermissonValidationService _PermissonValidationService) : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
    {
   
        public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
           
            var role = new Domain.RoleAgregate.Role( request.Name);
            if (request.PermissionIds.Any())await _PermissonValidationService.ValidatePermissions(request.PermissionIds, role, cancellationToken);
            await _roleRepository.AddRoleAsync(role, cancellationToken);
            await _roleRepository.SaveChangesAsync(cancellationToken);
            return new CreateRoleResponse(request.PermissionIds, role.Name);
        }
    }
}