using System;

namespace Application.Role.CreateRole
{
    public record CreateRoleResponse(ICollection<Guid> AssignedPermissions, string RoleName);
}