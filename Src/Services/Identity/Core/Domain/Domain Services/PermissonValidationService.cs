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
    public class PermissonValidationService(IPermissionRepository _PermissionRepository) : IPermissonValidationService
    {
        public async Task ValidatePermissions(IEnumerable<Guid> Permissonids, Role role, CancellationToken cancellationToken)
        {
            var Permission = await _PermissionRepository.GetPermissionIdsAsync(Permissonids, cancellationToken);
            role.AddPermissionIdsToRole(Permission.ToList());
        }

        
    }
}
