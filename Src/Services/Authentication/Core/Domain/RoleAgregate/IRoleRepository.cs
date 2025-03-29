using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RoleAgregate
{

    public interface IRoleRepository
    {
        // Role methods  
        Task AddRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int roleId);
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<IEnumerable<Role>> FindRolesAsync(Func<Role, bool> predicate);

        // Save changes  
        Task SaveChangesAsync(); // ذخیره تغییرات   
    }

}
