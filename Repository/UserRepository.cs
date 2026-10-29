using System;
using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Repository;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    //LINQ
    public async Task<IEnumerable<Users>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.RoleUsers).ThenInclude(ru => ru.Role).ThenInclude(r => r.PermissionRoles)
            .ToListAsync(); //select * from users
    }

    public async Task<Users?> GetusuarioById(int id)
    {
        return await _context.Users
            .Include(u => u.RoleUsers).ThenInclude(ru => ru.Role).ThenInclude(r => r.PermissionRoles)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <summary>
    /// Lectura sin tracking para armar la respuesta despues de crear/actualizar:
    /// evita que el identity map devuelva navegaciones (roles/permisos) desactualizadas.
    /// </summary>
    public async Task<Users?> GetUsuarioForResponse(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.RoleUsers).ThenInclude(ru => ru.Role).ThenInclude(r => r.PermissionRoles)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> ExistsByUsernameOrEmail(string username, string email, int? excludeId = null)
    {
        return await _context.Users.AnyAsync(u =>
            (u.Name.ToLower() == username.ToLower() || u.Email.ToLower() == email.ToLower())
            && (excludeId == null || u.Id != excludeId));
    }

    public async Task<Users> CreateUsuario(Users usuario)
    {
        _context.Users.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Users> UpdateUsuario(Users usuario)
    {
        _context.Users.Update(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task ReplaceRoles(int userId, IEnumerable<int> roleIds)
    {
        var distinctRoleIds = roleIds.Distinct().ToList();

        var existing = await _context.Set<RoleUser>()
            .Where(ru => ru.UserId == userId)
            .ToListAsync();
        _context.Set<RoleUser>().RemoveRange(existing);

        foreach (var roleId in distinctRoleIds)
        {
            await _context.Set<RoleUser>().AddAsync(new RoleUser
            {
                UserId = userId,
                RoleId = roleId
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> RolesExist(IEnumerable<int> roleIds)
    {
        var distinctRoleIds = roleIds.Distinct().ToList();
        var found = await _context.Roles.CountAsync(r => distinctRoleIds.Contains(r.Id));
        return found == distinctRoleIds.Count;
    }

    public async Task DeleteAsync(int id)
    {
        var usuario = await _context.Users.FindAsync(id);
        if(usuario != null)
        {
            var refreshTokens = await _context.RefreshTokens.Where(t => t.UserId == id).ToListAsync();
            _context.RefreshTokens.RemoveRange(refreshTokens);

            var roleLinks = await _context.Set<RoleUser>().Where(ru => ru.UserId == id).ToListAsync();
            _context.Set<RoleUser>().RemoveRange(roleLinks);

            _context.Users.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }

    public async Task LogicalDeleteAsync(int id)
    {
        var usuario = await _context.Users.FindAsync(id);
        if(usuario != null)
        {
            usuario.State = false;
            _context.Users.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
