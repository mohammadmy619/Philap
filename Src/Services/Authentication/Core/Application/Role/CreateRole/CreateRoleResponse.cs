using System;

namespace Application.Role.CreateRole
{
    public record CreateRoleResponse(ICollection<Guid> AssignedRoles, string RoleName);
}