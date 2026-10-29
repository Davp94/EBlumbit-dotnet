using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Seeders;

public class RolSeeder(AppDbContext context)
{
    private static readonly string[] VendedorSubjects =
        ["compras", "ventas", "inventario", "clientes", "proveedores"];

    private static readonly string[] RrhhSubjects =
        ["users", "roles", "permissions"];

    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        var permisos = await _context.Permissions.ToListAsync();
        if (permisos.Count == 0)
            return;

        var admin = await EnsureRole("ADMIN", "Rol administrador del sistema, acceso total");
        var vendedor = await EnsureRole("VENDEDOR", "vendedor que realiza operaciones de compras y ventas");
        var rrhh = await EnsureRole("RRHH", "Usuario de recursos humanos que gestiona accesos");

        await EnsurePermissions(admin, permisos);
        await EnsurePermissions(vendedor, permisos.Where(p => VendedorSubjects.Contains(p.Subject)));
        await EnsurePermissions(rrhh, permisos.Where(p => RrhhSubjects.Contains(p.Subject)));
    }

    private async Task<Role> EnsureRole(string nombre, string descripcion)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == nombre);
        if (role != null)
            return role;

        role = new Role
        {
            Nombre = nombre,
            Descripcion = descripcion
        };
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        return role;
    }

    private async Task EnsurePermissions(Role role, IEnumerable<Permission> permissions)
    {
        var existingPermissionIds = await _context.PermissionRoles
            .Where(pr => pr.RoleId == role.Id)
            .Select(pr => pr.PermissionId)
            .ToListAsync();

        var links = permissions
            .Where(p => !existingPermissionIds.Contains(p.Id))
            .Select(p => new PermissionRole
            {
                RoleId = role.Id,
                PermissionId = p.Id
            })
            .ToList();

        if (links.Count == 0)
            return;

        await _context.PermissionRoles.AddRangeAsync(links);
        await _context.SaveChangesAsync();
    }
}
