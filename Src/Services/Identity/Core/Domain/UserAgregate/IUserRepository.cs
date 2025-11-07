using BuildingBlocks.Interface;
using Domain.RoleAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UserAgregate
{
    public interface IUserRepository: IRepository
    {
   
        Task AddUserAsync(User user,CancellationToken cancellationToken);
        Task UpdateUserAsync(User user, CancellationToken cancellationToken);
        Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<User> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllUsersAsync( CancellationToken cancellationToken);
        Task<IEnumerable<User>> FindUsersAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);
        Task<User> FindUserAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);

        // Save changes  
        Task SaveChangesAsync(CancellationToken cancellationToken); // ذخیره تغییرات   
    }
}
