using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.RoleAgregate;
using Domain.RoleAgregate.Exception;
using MediatR;

namespace Application.Role.CreateRole
{
    public class CreateRoleCommandHandler(IRoleRepository _roleRepository) : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
    {
   
        public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.Name == null)
            {
                throw new RoleNameIsNullException();  
            }
            var role = new Domain.RoleAgregate.Role( request.Name);
            //////todo : use domain service
            await _roleRepository.AddRoleAsync(role, cancellationToken);
            await _roleRepository.SaveChangesAsync(cancellationToken);
            return new CreateRoleResponse(request.PermissionIds, role.Name);
        }
    }
}