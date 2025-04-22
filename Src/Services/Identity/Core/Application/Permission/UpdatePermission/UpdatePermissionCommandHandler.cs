using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Domain.Domain_Services;
using Domain.PermissionAgregate;
using MediatR;

namespace Application.Permission.UpdatePermission
{
    public class UpdatePermissionCommandHandler(IPermissionRepository _permissionRepository, IRoleValidationService _RoleValidationService)
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
                await _RoleValidationService.ValidateRoleIdsAsync(request.RoleIds, existingPermission, cancellationToken);
            }

            await _permissionRepository.UpdatePermissionAsync(existingPermission, cancellationToken);

            await _permissionRepository.SaveChangesAsync(cancellationToken);

         
            return new UpdatePermissionResponse(existingPermission.Id, existingPermission.Name);
        }
    }
}