using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.CreateUser
{
    public record CreateUserCommand(string UserName, string Email, string Password,List<Guid> RoleId) : IRequest<CreateUserResponse>;
}
