using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Permission.UpdateAccessControl
{
    public record UpdateAccessControlResponse(Guid PermissionId, string Resource, string Action);

}
