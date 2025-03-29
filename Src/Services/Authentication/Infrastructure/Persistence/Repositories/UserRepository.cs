using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.RoleAgregate;
using Domain.UserAgregate;
using Microsoft.EntityFrameworkCore;
using Persistence; 


public class UserRepository(AuthenticationDbContext _context) : IUserRepository
{

 

    public async Task AddUserAsync(User user)
    {
        await _context.User.AddAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await Task.FromResult(_context.Entry(user).State = EntityState.Modified);

    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.User.FindAsync(userId);
        if (user != null)
        {
            _context.User.Remove(user);
        }
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _context.User.FindAsync(userId);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.User.ToListAsync();
    }

    public async Task<IEnumerable<User>> FindUsersAsync(Func<User, bool> predicate)
    {
        
        return await Task.FromResult(_context.User.AsQueryable().Where(predicate).ToList());
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();  
    }
}