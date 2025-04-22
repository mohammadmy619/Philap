using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.PermissionAgregate;
using MediatR;

namespace Application.Permission.CreateAccessControl
{
    public class CreateAccessControlCommandHandler(IPermissionRepository _permissionRepository)
          : IRequestHandler<CreateAccessControlCommand, CreateAccessControlResponse>
    {
        public async Task<CreateAccessControlResponse> Handle(CreateAccessControlCommand request, CancellationToken cancellationToken)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(request.PermissionId, cancellationToken);

            // 1. Create a new AccessControl object
            var accessControl = new AccessControl(request.Resource, request.Action);
            permission.AddAccessControl(accessControl);

            await _permissionRepository.UpdatePermissionAsync(permission, cancellationToken);

            // 3. Return the response
            return new CreateAccessControlResponse(request.PermissionId,accessControl.Resource, accessControl.Action);
        }
    }
}
