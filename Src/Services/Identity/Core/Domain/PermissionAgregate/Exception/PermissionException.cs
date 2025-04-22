using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PermissionAgregate.Exception
{
    public class PermissionIdIsInvalidException : DomainException
    {
        public PermissionIdIsInvalidException(string message = "Permission ID must be a valid integer, and greater than zero.", string code = "0864001")
            : base(message, code) { }
    }

    public class PermissionNameIsNullException : DomainException
    {
        public PermissionNameIsNullException(string message = "Permission name cannot be null or empty.", string code = "0864002")
            : base(message, code) { }
    } 
    public class DuplicateAccessControlException : DomainException
    {
        public DuplicateAccessControlException(string message = "Duplicate Access Control", string code = "0864003")
            : base(message, code) { }
    }
    public class NotfoundAccessControlException : DomainException
    {
        public NotfoundAccessControlException(string message = "found Access Control", string code = "0864013")
            : base(message, code) { }
    }
    public class RoleIdNotValidsException : DomainException
    {
        public RoleIdNotValidsException(string message = "PermissionIds Not Valids Exception", string code = "0814012")
            : base(message, code) { }
    }
}
