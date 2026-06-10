using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.LoginUser
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base("نام کاربری یا رمز عبور اشتباه است.")
        { }

        public InvalidCredentialsException(string message)
            : base(message)
        { }

        public InvalidCredentialsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
