using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using Domain.PermissionAgregate;
using Microsoft.EntityFrameworkCore;
using Persistence;

public class PermissionRepository(AuthenticationDbContext _context) : IPermissionRepository
{
   

   

    public async Task AddPermissionAsync(Permission permission)
    {
        await _context.Permission.AddAsync(permission);
    }

    public  async Task UpdatePermission(Permission permission)
    {
       await Task.FromResult(_context.Entry(permission).State = EntityState.Modified);

    }

    public async Task DeletePermissionAsync(int permissionId)
    {
        var permission = await _context.Permission.FindAsync(permissionId);
        if (permission is not null)
        {
            _context.Permission.Remove(permission);
        }
    }

    public async Task<Permission> GetPermissionByIdAsync(int permissionId)
    {
        return await _context.Permission.FindAsync(permissionId);
    }

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
    {
        return await _context.Permission.ToListAsync();
    }

    public async Task<IEnumerable<Permission>> FindPermissionsAsync(Func<Permission, bool> predicate)
    {
        return await Task.FromResult(_context.Permission.AsQueryable().Where(predicate));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync(); // ذخیره تغییرات  
    }
}