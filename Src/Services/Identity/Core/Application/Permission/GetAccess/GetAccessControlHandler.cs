using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.PermissionAgregate;
using MediatR;

namespace Application.Permission.GetAccess
{
    public class GetAccessControlHandler(IPermissionRepository _permissionRepository) : IRequestHandler<GetAccessControlQuery, List<GetAccessControlResponse>>
    {

        public async Task<List<GetAccessControlResponse>> Handle(GetAccessControlQuery request, CancellationToken cancellationToken)
        {
            var accessControls = await _permissionRepository
           .GetAllAccessControlAsync(request.permissionId, cancellationToken);

            var responses = accessControls
            .SelectMany(ac => ac.AccessControl.Select(item =>
                new GetAccessControlResponse(item.Action, item.Resource)))
            .ToList();

            return responses;
        }

    }
}
