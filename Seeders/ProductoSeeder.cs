using Bogus;
using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Seeders;

public class ProductoSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if (await _context.Productos.AnyAsync())
            return;

        var categorias = await _context.Categorias.OrderBy(c => c.Id).ToListAsync();
        if (categorias.Count == 0)
            return;

        var faker = new Faker("es");
        var productos = Enumerable.Range(1, 15).Select(index => new Productos
        {
            Nombre = faker.Commerce.ProductName(),
            CodigoBarra = $"779{index:010}",
            UnidadMedida = faker.PickRandom("unidad", "paquete", "litro", "kilogramo"),
            Marca = faker.Company.CompanyName(),
            Imagen = string.Empty,
            Descripcion = faker.Commerce.ProductDescription(),
            PrecioVentaActual = faker.Random.Decimal(5, 500),
            StockMinimo = faker.Random.Int(5, 25),
            Estado = true,
            FechaRegistro = DateTime.UtcNow,
            CategoriaId = categorias[(index - 1) % categorias.Count].Id
        });

        await _context.Productos.AddRangeAsync(productos);
        await _context.SaveChangesAsync();
    }
}