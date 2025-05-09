using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace Application.Role.GetRole
{
    internal class RolesNotFoundException:DomainException
    {
        public RolesNotFoundException(string message = "Roles Not Found", string code = "0214024")
           : base(message, code) { }
    }
}
