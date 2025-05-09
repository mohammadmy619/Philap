using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Permission.CreatePermission
{
    public record CreatePermissionCommand(string Name,List<Guid> RoleIds) : IRequest<CreatePermissionResponse>;
   
}
