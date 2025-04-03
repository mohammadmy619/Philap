using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Role.UpdateRole
{
    public record UpdateRoleCommand(Guid RoleId, string Name, List<Guid> PermissionIds) : IRequest<UpdateRoleResponse>;

}
