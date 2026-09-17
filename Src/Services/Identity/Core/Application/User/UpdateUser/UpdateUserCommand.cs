using System;
using MediatR;

namespace Application.User.UpdateUser
{
    public record UpdateUserCommand(string UserName, string Email, string Password, List<Guid> RoleId) : IRequest<UpdateUserResponse>;
}
