using Domain.PermissionAgregate;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Permission.CreatePermission
{
    public class CreatePermissionCommandHandler(IPermissionRepository _permissionRepository) : IRequestHandler<CreatePermissionCommand, CreatePermissionResponse>
    {
      

        public async Task<CreatePermissionResponse> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
           
            var permission = new Domain.PermissionAgregate.Permission(request.Name);

       
            await _permissionRepository.AddPermissionAsync(permission, cancellationToken);
            await _permissionRepository.SaveChangesAsync(cancellationToken);

           
            return new CreatePermissionResponse(permission.Id, permission.Name);
        }
    }
}