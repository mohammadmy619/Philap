using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PermissionAgregate.Exception
{
    public class AccessControlResourceInvalidException : DomainException
    {
        public AccessControlResourceInvalidException(string message = "Resource cannot be null or empty.", string code = "0864002")
            : base(message, code) { }
    }

    public class AccessControlActionInvalidException : DomainException
    {
        public AccessControlActionInvalidException(string message = "Action cannot be null or empty.", string code = "0864003")
            : base(message, code) { }
    }
    public class AccessControlNotFoundException : DomainException
    {
        public AccessControlNotFoundException(string message = "Access control not found.", string code = "0864004")
            : base(message, code) { }
    }
    public class AccessControlIsNullException : DomainException
    {
        public AccessControlIsNullException(string message = "Access control cannot be null.", string code = "0864005")
            : base(message, code) { }
    }
}
