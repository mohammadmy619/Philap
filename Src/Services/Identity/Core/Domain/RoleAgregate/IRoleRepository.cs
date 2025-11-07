using BuildingBlocks.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RoleAgregate
{

    public interface IRoleRepository: IRepository
    {
        // Role methods  
        Task AddRoleAsync(Role role,CancellationToken cancellationToken);
        Task UpdateRoleAsync(Role role, CancellationToken cancellationToken);
        Task DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken);
        Task<Role> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken);
        Task<IEnumerable<Guid>> GetRoleIdsAsync(IEnumerable<Guid> roleId, CancellationToken cancellationToken);
        Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Role>> FindRolesAsync(Expression<Func<Role, bool>> predicate, CancellationToken cancellationToken);


        // Save changes  
        Task SaveChangesAsync(CancellationToken cancellationToken); // ذخیره تغییرات   
    }

}
