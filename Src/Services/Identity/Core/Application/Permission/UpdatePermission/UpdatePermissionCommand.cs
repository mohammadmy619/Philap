using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Permission.UpdatePermission
{
    public record UpdatePermissionCommand(Guid Id, string Name, List<Guid> RoleIds) : IRequest<UpdatePermissionResponse>;

}
