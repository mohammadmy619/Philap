using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.PermissionAgregate;
using Domain.RoleAgregate;
using Domain.UserAgregate;
using Microsoft.EntityFrameworkCore;
using Persistence;

public class RoleRepository(IdentityDbContext _context) : IRoleRepository
{


    public async Task AddRoleAsync(Role role, CancellationToken cancellationToken)
    {
        await _context.Role.AddAsync(role, cancellationToken);
    }

    public async Task UpdateRoleAsync(Role role, CancellationToken cancellationToken)
    {
        await Task.Run(() => { return _context.Entry(role).State = EntityState.Modified; }, cancellationToken);


    }

    public async Task DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _context.Role.FindAsync(roleId, cancellationToken);
        if (role is not null)
        {
            _context.Role.Remove(role);

        }
    }

    public async Task<Role> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken)
    {
        return await _context.Role.FindAsync(roleId, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        return await _context.Role.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Role>> FindRolesAsync(Func<Role, bool> predicate, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            return _context.Role.AsQueryable().Where(predicate).ToList();
        }, cancellationToken);
    }
    public async Task<IEnumerable<Guid>> GetRoleIdsAsync(IEnumerable<Guid> roleId, CancellationToken cancellationToken)
    {
        return await _context.Role
        .Where(role => roleId.Contains(role.Id))
        .Select(s => s.Id)
        .ToListAsync(cancellationToken);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }


}