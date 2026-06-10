using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationServices
{
    public class PasswordHelper : IPasswordHelper
    {


        public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyPassword(string hashedPassword, string plainPassword) =>
            BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}
