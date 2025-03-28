using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RoleAgregate.Exception
{
    public class UserRoleIdIsInvalidException : DomainException
    {
        public UserRoleIdIsInvalidException(string message = "UserRole ID must be a valid integer and greater than zero.", string code = "0844001")
            : base(message, code) { }
    }

    public class UserIdIsInvalidException : DomainException
    {
        public UserIdIsInvalidException(string message = "User ID must be a valid integer and greater than zero.", string code = "0844002")
            : base(message, code) { }
    }

    
}
