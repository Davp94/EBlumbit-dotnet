using Bogus;
using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Seeders;

public class SucursalSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if (await _context.Sucursales.AnyAsync())
            return;

        var faker = new Faker("es");
        var ciudades = new[] { "La Paz", "Cochabamba", "Santa Cruz" };
        var sucursales = ciudades.Select((ciudad, index) => new Sucursales
        {
            Nombre = $"Sucursal {ciudad}",
            Direccion = faker.Address.FullAddress(),
            Telefono = $"+591 7{index + 1}2345678",
            Ciudad = ciudad
        });

        await _context.Sucursales.AddRangeAsync(sucursales);
        await _context.SaveChangesAsync();
    }
}