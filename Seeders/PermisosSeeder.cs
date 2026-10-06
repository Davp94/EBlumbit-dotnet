using System;
using EBlumbit.Data;
using EBlumbit.Models;

namespace EBlumbit.Seeders;

public class PermisosSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if (_context.Permissions.Any())
            return;

        var subjects = new[]
        {
            "users", "roles", "permissions", "productos", "compras", "ventas",
            "inventario", "sucursales", "almacenes", "clientes", "proveedores"
        };

        var actions = new[] { "crear", "leer", "actualizar", "eliminar" };

        var permissions = new List<Permission>();
        foreach (var subject in subjects)
        {
            foreach (var action in actions)
            {
                permissions.Add(new Permission
                {
                    Nombre = $"{action}:{subject}",
                    Subject = subject,
                    Action = action,
                    Detalle = "asignacion masiva"
                });
            }
        }
        //custom permissions
        permissions.Add(new Permission
        {
            Nombre = "ejecutivo",
            Subject = "eblumbit",
            Action = "leer",
            Detalle = "Permiso ejecutivo para ver informacion general de la app"
        });

        permissions.Add(new Permission
        {
            Nombre = "admin:ventas",
            Subject = "ventas",
            Action = "administrar",
            Detalle = "Permiso para realizar todas las operaciones en ventas"
        });
        await _context.Permissions.AddRangeAsync(permissions);
        await _context.SaveChangesAsync();
    }

}
