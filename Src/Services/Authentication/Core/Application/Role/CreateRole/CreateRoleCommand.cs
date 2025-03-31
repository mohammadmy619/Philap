using MediatR;

namespace Application.Role.CreateRole
{
    public record CreateRoleCommand(string Name, ICollection<Guid> PermissionIds) : IRequest<CreateRoleResponse>;
}