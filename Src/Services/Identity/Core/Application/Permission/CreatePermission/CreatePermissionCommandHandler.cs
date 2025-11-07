using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Permission.CreatePermission
{
    public class CreatePermissionCommandHandler(IPermissionRepository _permissionRepository, IRoleRepository _RoleRepository) : IRequestHandler<CreatePermissionCommand, CreatePermissionResponse>
    {
      

        public async Task<CreatePermissionResponse> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
           
            var permission = new Domain.PermissionAgregate.Permission(request.Name);

            if (request.RoleIds.Any() && request.RoleIds.Count > 0)
            {
                var roles = await _RoleRepository.GetRoleIdsAsync(request.RoleIds, cancellationToken);
                permission.AddRoleIds(roles.ToList());
                
            }
       
            await _permissionRepository.AddPermissionAsync(permission, cancellationToken);
            await _permissionRepository.SaveChangesAsync(cancellationToken);
            return new CreatePermissionResponse(permission.Id, permission.Name);
        }
    }
}