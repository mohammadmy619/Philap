using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using Microsoft.EntityFrameworkCore;
using Persistence;

public class PermissionRepository(IdentityDbContext _context) : IPermissionRepository
{
   

    public async Task AddPermissionAsync(Permission permission, CancellationToken cancellationToken)
    {
        await _context.Permission.AddAsync(permission, cancellationToken);
    }

    public async Task UpdatePermissionAsync(Permission permission, CancellationToken cancellationToken)
    {

        _context.Entry(permission).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task DeletePermissionAsync(int permissionId, CancellationToken cancellationToken)
    {
        var permission = await _context.Permission.FindAsync(new object[] { permissionId }, cancellationToken);
        if (permission != null)
        {
            _context.Permission.Remove(permission);
        }
        else
        {
            throw new KeyNotFoundException($"Permission with ID {permissionId} not found");
        }
    }

    public async Task<Permission> GetPermissionByIdAsync(int permissionId, CancellationToken cancellationToken)
    {
        return await _context.Permission.FindAsync(new object[] { permissionId }, cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken)
    {
        return await _context.Permission.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Permission>> FindPermissionsAsync(Func<Permission, bool> predicate, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            return _context.Permission.AsQueryable().Where(predicate).ToList();
        }, cancellationToken);
    }
    public  async Task<IEnumerable<Guid>> GetPermissionIdsAsync(IEnumerable<Guid> PermissionId, CancellationToken cancellationToken)
    {
        return await _context.Permission
     .Where(role => PermissionId.Contains(role.Id))
     .Select(s => s.Id)
     .ToListAsync(cancellationToken);
    }

   
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken); // ذخیره تغییرات  
    }

   
}