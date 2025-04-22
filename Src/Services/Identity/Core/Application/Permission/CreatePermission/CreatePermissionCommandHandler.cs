using Domain.Domain_Services;
using Domain.PermissionAgregate;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Permission.CreatePermission
{
    public class CreatePermissionCommandHandler(IPermissionRepository _permissionRepository, IRoleValidationService _RoleValidationService) : IRequestHandler<UpdatePermissionCommand, CreatePermissionResponse>
    {
      

        public async Task<CreatePermissionResponse> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
           
            var permission = new Domain.PermissionAgregate.Permission(request.Name);

            if (request.RoleIds.Any() && request.RoleIds.Count > 0)
            {
               await _RoleValidationService.ValidateRoleIdsAsync(request.RoleIds,permission, cancellationToken);
            }
       
            await _permissionRepository.AddPermissionAsync(permission, cancellationToken);
            await _permissionRepository.SaveChangesAsync(cancellationToken);

           
            return new CreatePermissionResponse(permission.Id, permission.Name);
        }
    }
}