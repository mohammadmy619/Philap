using Domain.RoleAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UserAgregate
{
    public interface IUserRepository
    {
        // User methods  
        Task AddUserAsync(User user); 
        Task UpdateUserAsync(User user); 
        Task DeleteUserAsync(Guid userId);  
        Task<User> GetUserByIdAsync(Guid userId);   
        Task<User> GetUserByEmailAsync(string email);   
        Task<IEnumerable<User>> GetAllUsersAsync();   

        // Role methods  
        Task AddRoleAsync(RoleAgregate.Role role);   
        Task UpdateRoleAsync(RoleAgregate.Role role);  
        Task DeleteRoleAsync(int roleId);  
        Task<RoleAgregate.Role> GetRoleByIdAsync(int roleId); 
        Task<IEnumerable<RoleAgregate.Role>> GetAllRolesAsync();   

        // User-Role methods  
        Task AddUserRoleAsync(UserRole userRole); 
        Task DeleteUserRoleAsync(int userRoleId);   
        Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(Guid userId); 

        // Permissions methods  
        Task AddPermissionAsync(Permission permission);  
        Task UpdatePermissionAsync(Permission permission); 
        Task DeletePermissionAsync(int permissionId); 
        Task<Permission> GetPermissionByIdAsync(int permissionId); 
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();  
        Task<IEnumerable<Permission>> FindPermissionsAsync(Func<Permission, bool> predicate); 

        // Save changes  
        Task SaveChangesAsync(); // ذخیره تغییرات 
    }
}
