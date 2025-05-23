﻿using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UserAgregate.Exception
{
    public class UserIdIsInvalidException : DomainException
    {
        public UserIdIsInvalidException(string message = "User ID must be a valid GUID.", string code = "0814001")
            : base(message, code) { }
    }

    public class UserNameIsNullException : DomainException
    {
        public UserNameIsNullException(string message = "User name cannot be null or empty.", string code = "0814002")
            : base(message, code) { }
    }

    public class EmailIsNullException : DomainException
    {
        public EmailIsNullException(string message = "Email cannot be null or empty.", string code = "0814003")
            : base(message, code) { }
    }
    public class InvalidEmailException : DomainException
    {
        public InvalidEmailException(string message = " The provided email is not in a valid format.", string code = "0814003")
            : base(message, code) { }
    }

    public class PasswordHashIsNullException : DomainException
    {
        public PasswordHashIsNullException(string message = "Password hash cannot be null or empty.", string code = "0814004")
            : base(message, code) { }
    }
    public class RoleIdNotValidsException : DomainException
    {
        public RoleIdNotValidsException(string message = "PermissionIds Not Valids Exception", string code = "0814012")
            : base(message, code) { }
    }
}
