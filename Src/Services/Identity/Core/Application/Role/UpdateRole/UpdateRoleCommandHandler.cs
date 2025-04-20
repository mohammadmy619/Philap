using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.RoleAgregate;
using MediatR;
using Domain.Domain_Services;
using System.IO;
using Domain.RoleAgregate.Exception;

namespace Application.Role.UpdateRole
{
    public class UpdateRoleCommandHandler(IRoleRepository _roleRepository, IPermissonValidationService _permissionValidationService) : IRequestHandler<UpdateRoleCommand, UpdateRoleResponse>
    {

        public async Task<UpdateRoleResponse> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            // Fetch the existing role  
            var role = await _roleRepository.GetRoleByIdAsync(request.RoleId, cancellationToken);
            if (role == null)
                throw new RoleIdIsInvalidException($"Role with ID {request.RoleId} not found.");


            role.UpdateRole(request.RoleId, request.Name);


            // Validate permissions if any permission IDs are provided  
            if (request.PermissionIds != null && request.PermissionIds.Any())
            {
                await _permissionValidationService.ValidatePermissions(request.PermissionIds, role, cancellationToken);
                role.AddPermissionIdsToRole(request.PermissionIds); 
            }

            await _roleRepository.UpdateRoleAsync(role, cancellationToken);
            await _roleRepository.SaveChangesAsync(cancellationToken);

            // Return the response  
            return new UpdateRoleResponse(role.Id, role.Name, role.PermissionIds);
        }
    }
}