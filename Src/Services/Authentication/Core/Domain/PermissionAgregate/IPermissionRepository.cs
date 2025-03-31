using BuildingBlocks.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PermissionAgregate
{
   public interface IPermissionRepository: IRepository
    {
        // Permissions methods  
        Task AddPermissionAsync(Permission permission, CancellationToken cancellationToken);
        Task UpdatePermissionAsync(Permission permission, CancellationToken cancellationToken);
        Task DeletePermissionAsync(int permissionId, CancellationToken cancellationToken);
        Task<Permission> GetPermissionByIdAsync(int permissionId, CancellationToken cancellationToken);
        Task<IEnumerable<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Permission>> FindPermissionsAsync(Func<Permission, bool> predicate, CancellationToken cancellationToken);

        // Save changes  
        Task SaveChangesAsync(CancellationToken cancellationToken); // ذخیره تغییرات 
    }
}
