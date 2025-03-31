using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Domain.RoleAgregate;
using Domain.UserAgregate;
using Microsoft.EntityFrameworkCore;
using Persistence;


public class UserRepository(AuthenticationDbContext _context) : IUserRepository
{

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await _context.User.AddAsync(user, cancellationToken);
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        await Task.Run(() => { return _context.Entry(user).State = EntityState.Modified; }, cancellationToken);

    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _context.User.FindAsync(userId, cancellationToken);
        if (user != null)
        {
            _context.User.Remove(user);
        }
    }

    public async Task<User> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.User.FindAsync(userId, cancellationToken);
     
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        return await _context.User.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> FindUsersAsync(Func<User, bool> predicate, CancellationToken cancellationToken)
    {

        return await Task.Run(() =>
        {
            return _context.User.AsQueryable().Where(predicate).ToList();
        }, cancellationToken);

    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}