using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using Microsoft.EntityFrameworkCore;
using Persistence; 

public class RoleRepository(AuthenticationDbContext _context) : IRoleRepository
{
  

    public async Task AddRoleAsync(Role role)
    {
        await _context.Role.AddAsync(role);
    }

    public async Task UpdateRoleAsync(Role role)
    {
        await Task.FromResult(_context.Entry(role).State = EntityState.Modified);
        

    }

    public async Task DeleteRoleAsync(int roleId)
    {
        var role = await _context.Role.FindAsync(roleId);
        if (role is not null)
        {
            _context.Role.Remove(role);
    
        }
    }

    public async Task<Role> GetRoleByIdAsync(int roleId)
    {
        return await _context.Role.FindAsync(roleId);
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _context.Role.ToListAsync();
    }

    public async Task<IEnumerable<Role>> FindRolesAsync(Func<Role, bool> predicate)
    {
        return await Task.FromResult(_context.Role.AsQueryable().Where(predicate).ToList());
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();  
    }
}