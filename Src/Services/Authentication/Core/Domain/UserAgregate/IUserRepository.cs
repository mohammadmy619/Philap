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
   
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> FindUsersAsync(Func<User, bool> predicate);

        // Save changes  
        Task SaveChangesAsync(); // ذخیره تغییرات   
    }
}
