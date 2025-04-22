using BuildingBlocks.Domain;
using BuildingBlocks.Interface;
using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using Domain.UserAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain_Services
{
   public class RoleValidationService(IRoleRepository _RoleRepository) : IRoleValidationService, IDomainService
    {

        public async Task ValidateRoleIdsAsync(IEnumerable<Guid> roleIds, User user,CancellationToken cancellationToken)
        {
        
            var roles = await _RoleRepository.GetRoleIdsAsync(roleIds, cancellationToken);
            user.AddRolesToUser(roles.ToList());
          

        }
        public async Task ValidateRoleIdsAsync(IEnumerable<Guid> roleIds, Permission permission, CancellationToken cancellationToken)
        {
        
            var roles = await _RoleRepository.GetRoleIdsAsync(roleIds, cancellationToken);
            permission.AddRoleIds(roles.ToList());
        }


        public Task ValidateRoleIdsAndUpdateAsync(IEnumerable<Guid> roleIds, Permission permission, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ValidateRoleIdsAndUpdateAsync(IEnumerable<Guid> roleIds, User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
