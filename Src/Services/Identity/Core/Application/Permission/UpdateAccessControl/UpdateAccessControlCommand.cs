using Application.Permission.UpdateAccessControl;
using Application.Permission.UpdatePermission;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Permission.CreatePermission
{
    public record UpdateAccessControlCommand(Guid PermissionId, string Resource, string Action) : IRequest<UpdateAccessControlResponse>;
   
}
