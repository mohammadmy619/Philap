using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PermissionAgregate
{
   public interface IPermissionRepository
    {
        // Permissions methods  
        Task AddPermissionAsync(Permission permission);
        Task UpdatePermission(Permission permission);
        Task DeletePermissionAsync(int permissionId);
        Task<Permission> GetPermissionByIdAsync(int permissionId);
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
        Task<IEnumerable<Permission>> FindPermissionsAsync(Func<Permission, bool> predicate);

        // Save changes  
        Task SaveChangesAsync(); // ذخیره تغییرات 
    }
}
