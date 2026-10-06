using System;
using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Seeders;

public class RolSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if(_context.Roles.Any())
            return;
        
        var permisos = await _context.Permissions.ToListAsync();

        var admin = new Role
        {
            Nombre = "ADMIN",
            Descripcion = "Rol administrador del sistema, acceso total",
            Permisos = permisos
        };

        var vendedorSubjects = new[]{"compras", "ventas",
            "inventario", "clientes", "proveedores", "ventas"};
        var vendedorPermisos = permisos.Where(p => vendedorSubjects.Contains(p.Subject)).ToList();
        var vendedor = new Role
        {
            Nombre = "VENDEDOR",
            Descripcion = "vendedor que realiza operaciones de compras y ventas",
            Permisos = vendedorPermisos
        };

        var rrhhSubjects = new[]{"users", "roles", "permissions"};
        var rrhhPermisos = permisos.Where(p => rrhhSubjects.Contains(p.Subject)).ToList();
        var rrhh = new Role
        {
            Nombre = "RRHH",
            Descripcion = "Usuario de recursos humanos que gestiona accesos",
            Permisos = rrhhPermisos
        };

        await _context.Roles.AddRangeAsync(admin, vendedor, rrhh);
        await _context.SaveChangesAsync();
    }
}
