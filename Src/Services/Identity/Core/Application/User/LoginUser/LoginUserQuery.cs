using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.User.LoginUser
{
    
    public record LoginUserQuery(string Username, string Password):IRequest<LoginUserResponse>;
   
}
