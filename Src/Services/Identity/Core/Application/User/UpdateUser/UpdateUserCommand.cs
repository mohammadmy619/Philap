using System;
using MediatR;

namespace Application.User.UpdateUser
{
    public record UpdateUserCommand(Guid UserId, string UserName, string Email, string Password, List<Guid> RoleId) : IRequest<UpdateUserResponse>;
}
