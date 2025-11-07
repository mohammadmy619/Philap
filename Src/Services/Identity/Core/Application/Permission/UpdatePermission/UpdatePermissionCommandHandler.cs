using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using MediatR;

namespace Application.Permission.UpdatePermission
{
    public class UpdatePermissionCommandHandler(IPermissionRepository _permissionRepository, IRoleRepository _RoleRepository)
        : IRequestHandler<UpdatePermissionCommand, UpdatePermissionResponse>
    {
        public async Task<UpdatePermissionResponse> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
        
            var existingPermission = await _permissionRepository.GetPermissionByIdAsync(request.Id, cancellationToken);

            if (existingPermission == null)
            {
                throw new InvalidOperationException($"Permission with Id {request.Id} not found.");
            }

        
            existingPermission.UpdatePermission(request.Id,request.Name);

            if (request.RoleIds.Any() && request.RoleIds.Count > 0)
            {
                var roles = await _RoleRepository.GetRoleIdsAsync(request.RoleIds, cancellationToken);
                existingPermission.AddRoleIds(roles.ToList());
               
            }

            await _permissionRepository.UpdatePermissionAsync(existingPermission, cancellationToken);

            await _permissionRepository.SaveChangesAsync(cancellationToken);

         
            return new UpdatePermissionResponse(existingPermission.Id, existingPermission.Name);
        }
    }
}