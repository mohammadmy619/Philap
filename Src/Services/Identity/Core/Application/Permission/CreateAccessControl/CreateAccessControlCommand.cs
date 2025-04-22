using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Permission.CreateAccessControl
{
    public record CreateAccessControlCommand(Guid PermissionId,string Resource, string Action) : IRequest<CreateAccessControlResponse>;

}
