using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Permission.GetPermission
{
    public record class GetPermissionResponse(Guid permissonId,string Name,IReadOnlyCollection<Guid> RoleId);

}
