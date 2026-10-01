using Bogus;
using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Seeders;

public class AlmacenSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if (await _context.Almacenes.AnyAsync())
            return;

        var sucursales = await _context.Sucursales.OrderBy(s => s.Id).ToListAsync();
        var faker = new Faker("es");
        var almacenes = sucursales.Select((sucursal, index) => new Almacenes
        {
            Codigo = $"ALM-{index + 1:000}",
            Nombre = $"Almacén {sucursal.Ciudad}",
            Descripcion = faker.Lorem.Sentence(),
            SucursalId = sucursal.Id
        });

        await _context.Almacenes.AddRangeAsync(almacenes);
        await _context.SaveChangesAsync();
    }
}