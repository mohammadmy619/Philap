using System;
using System.Collections.Generic;

namespace Application.User.GetUser
{
    public record GetUserResponse(
        Guid UserId,
        string UserName,
        string Email,
        DateTime CreatedAt,
        List<Guid> AssignedRoles);
}