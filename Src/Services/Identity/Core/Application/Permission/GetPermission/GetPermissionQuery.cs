using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Permission.GetPermission
{
    public record class GetPermissionQuery(Guid PermissionId):IRequest<GetPermissionResponse>;
}
   
