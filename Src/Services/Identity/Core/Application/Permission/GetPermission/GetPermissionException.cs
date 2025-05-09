using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace Application.Permission.GetPermission
{
    public class GetPermissionException:DomainException
    {
        public GetPermissionException(string message = "Permission Not Found", string code = "0044021")
        : base(message, code) { }
    }
}
