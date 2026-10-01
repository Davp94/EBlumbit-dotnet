using Bogus;
using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Seeders;

public class InventarioSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if (await _context.Inventarios.AnyAsync())
            return;

        var productos = await _context.Productos.ToListAsync();
        var almacenes = await _context.Almacenes.ToListAsync();
        var faker = new Faker("es");
        var inventarios = from producto in productos
                          from almacen in almacenes
                          select new Inventario
                          {
                              ProductoId = producto.Id,
                              AlmacenId = almacen.Id,
                              CantidadActual = faker.Random.Int(10, 200),
                              FechaActualizacion = DateTime.UtcNow
                          };

        await _context.Inventarios.AddRangeAsync(inventarios);
        await _context.SaveChangesAsync();
    }
}