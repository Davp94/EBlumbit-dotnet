using Bogus;
using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Seeders;

public class CategoriaSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if (await _context.Categorias.AnyAsync())
            return;

        var faker = new Faker("es");
        var categorias = new[]
        {
            "Bebidas", "Abarrotes", "Limpieza", "Cuidado personal", "Snacks"
        }.Select(nombre => new Categoria
        {
            Nombre = nombre,
            Detalle = faker.Lorem.Sentence()
        });

        await _context.Categorias.AddRangeAsync(categorias);
        await _context.SaveChangesAsync();
    }
}