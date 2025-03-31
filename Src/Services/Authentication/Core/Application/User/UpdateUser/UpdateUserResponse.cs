using System;

namespace Application.User.UpdateUser
{
    public record UpdateUserResponse(Guid UserId, string UserName, string Email);
}