using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.User.Exceptions;
using Application.User.GetUser;
using Domain.RoleAgregate;
using Domain.UserAgregate;
using MediatR;

namespace Application.Role.GetRole
{
    public class GetRoleQueryHandler(IRoleRepository _RoleRepository) : IRequestHandler<GetRoleQuery, GetRoleResponse>
    {


        public  async Task<GetRoleResponse> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {

            // دریافت کاربر از repo  
            var Roles = await _RoleRepository.GetAllRolesAsync(cancellationToken);

            if (Roles == null)
            {
                throw new RolesNotFoundException();
            }
            // جمع‌آوری تمام PermissionIds از تمام نقش‌ها
            var allPermissionIds = Roles
                .SelectMany(r => r.PermissionIds) 
                .ToList();

            
            var assignedRoleIds = Roles.Select(o=>o.Id)?.ToList() ?? new List<Guid>();

            var response = new GetRoleResponse(
                PermissionIds: allPermissionIds,
                RoleId: assignedRoleIds
            );
            return response;

        }

      
    }
}
