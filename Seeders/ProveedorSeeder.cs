using Bogus;
using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Seeders;

public class ProveedorSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if (await _context.Proveedores.AnyAsync())
            return;

        var faker = new Faker("es");
        var proveedores = Enumerable.Range(1, 8).Select(index => new Proveedor
        {
            RazonSocial = $"{faker.Company.CompanyName()} S.R.L.",
            NroIdentificacion = $"NIT-{index:0000000}",
            Contacto = faker.Name.FullName(),
            Telefono = faker.Phone.PhoneNumber(),
            Correo = faker.Internet.Email(),
            Observaciones = faker.Lorem.Sentence(),
            Estado = true
        });

        await _context.Proveedores.AddRangeAsync(proveedores);
        await _context.SaveChangesAsync();
    }
}