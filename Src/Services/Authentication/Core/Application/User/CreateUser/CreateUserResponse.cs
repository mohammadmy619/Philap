using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.CreateUser
{
    public record CreateUserResponse(
    Guid UserId,
    string UserName,
    string Email,
    DateTime CreatedAt,
    List<Guid> AssignedRoles);
}
