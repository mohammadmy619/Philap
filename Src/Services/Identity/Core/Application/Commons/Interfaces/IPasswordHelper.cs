using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Interfaces
{
   public interface IPasswordHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string plainPassword);
        //public string EncodePasswordMd5(string pass);
    }
}
