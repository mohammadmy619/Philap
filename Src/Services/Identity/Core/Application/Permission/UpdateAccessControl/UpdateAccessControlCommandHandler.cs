using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Permission.CreatePermission;
using Domain.PermissionAgregate;
using MediatR;

namespace Application.Permission.UpdateAccessControl
{

    public class UpdateAccessControlCommandHandler(IPermissionRepository _permissionRepository)
        : IRequestHandler<UpdateAccessControlCommand, UpdateAccessControlResponse>
    {
        public async Task<UpdateAccessControlResponse> Handle(UpdateAccessControlCommand request, CancellationToken cancellationToken)
        {

            // 1. Find the existing AccessControl by Resource and Action
            var existingAccessControl = await _permissionRepository.GetPermissionByIdAsync(request.PermissionId, cancellationToken);

            if (existingAccessControl == null)
            {
                throw new InvalidOperationException($"Permission not found.");
            }

            existingAccessControl.UpdateAccessControl(new  AccessControl(request.Resource,request.Action));

            // 3. Save the updated AccessControl
            await _permissionRepository.UpdatePermissionAsync(existingAccessControl, cancellationToken);
            await _permissionRepository.SaveChangesAsync(cancellationToken);

            // 4. Return the response
            return new UpdateAccessControlResponse(request.PermissionId, request.Resource, request.Action);
        }
    }
}
