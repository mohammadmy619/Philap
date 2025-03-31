using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.services
{
   public interface IPasswordHelper
    {
        public string EncodePasswordMd5(string pass);
    }
}
