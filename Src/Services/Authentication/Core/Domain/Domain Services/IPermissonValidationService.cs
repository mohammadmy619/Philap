using BuildingBlocks.Interface;
using Domain.RoleAgregate;
using Domain.UserAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain_Services
{
    public interface IPermissonValidationService:IDomainService
    {
        public Task ValidatePermissions(IEnumerable<Guid> Permissonids, Role role, CancellationToken cancellationToken);
    }
}
