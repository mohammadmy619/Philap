using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace Application.Permission.GetAccess
{
    public class NotFoundAccessControlException:DomainException
    {
        public NotFoundAccessControlException(string message = "Access Control Not Found", string code = "0044021")
            : base(message, code) { }
    }
}
