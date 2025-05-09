using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.PermissionAgregate;
using MediatR;

namespace Application.Permission.GetPermission
{
    public class GetPermissionHandler(IPermissionRepository _PermissionRepository) : IRequestHandler<GetPermissionQuery, GetPermissionResponse>
    {
        public async Task<GetPermissionResponse> Handle(GetPermissionQuery request, CancellationToken cancellationToken)
        {
            var permission =await _PermissionRepository.GetPermissionByIdAsync(request.PermissionId, cancellationToken);
            if (permission != null) { throw new GetPermissionException(); }
            return new GetPermissionResponse(permission.Id,permission.Name,permission.RoleIds);
        }
    }
}
