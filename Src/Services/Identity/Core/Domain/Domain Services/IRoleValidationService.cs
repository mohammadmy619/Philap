using BuildingBlocks.Interface;
using Domain.PermissionAgregate;
using Domain.UserAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain_Services
{
   public interface IRoleValidationService: IDomainService
    {
        public Task ValidateRoleIdsAsync(IEnumerable<Guid> roleIds,User user, CancellationToken cancellationToken);
    
        public Task ValidateRoleIdsAsync(IEnumerable<Guid> roleIds, Permission permission, CancellationToken cancellationToken);

    }
}
