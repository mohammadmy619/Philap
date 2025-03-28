using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RoleAgregate.Exception
{
    public class RoleIdIsInvalidException : DomainException
    {
        public RoleIdIsInvalidException(string message = "Role ID must be a valid integer and greater than zero.", string code = "0874001")
            : base(message, code) { }
    }

    public class RoleNameIsNullException : DomainException
    {
        public RoleNameIsNullException(string message = "Role name cannot be null or empty.", string code = "0874002")
            : base(message, code) { }
    }
}
