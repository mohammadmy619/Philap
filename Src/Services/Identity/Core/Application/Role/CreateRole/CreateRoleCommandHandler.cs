using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using Domain.RoleAgregate.Exception;
using MediatR;

namespace Application.Role.CreateRole
{
    public class CreateRoleCommandHandler(IRoleRepository _roleRepository,IPermissionRepository _PermissionRepository) : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
    {
   
        public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
           
            var role = new Domain.RoleAgregate.Role( request.Name);
            if (request.PermissionIds.Any()) {

                var Permission = await _PermissionRepository.GetPermissionIdsAsync(role.PermissionIds, cancellationToken);
                role.AddPermissionIdsToRole(Permission.ToList());

            }
 
            await _roleRepository.AddRoleAsync(role, cancellationToken);
            await _roleRepository.SaveChangesAsync(cancellationToken);
            return new CreateRoleResponse(request.PermissionIds, role.Name);
        }
    }
}