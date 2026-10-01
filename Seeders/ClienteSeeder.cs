using Bogus;
using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Seeders;

public class ClienteSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if (await _context.Clientes.AnyAsync())
            return;

        var faker = new Faker("es");
        var clientes = Enumerable.Range(1, 15).Select(index => new Cliente
        {
            NombreCompleto = faker.Name.FullName(),
            NroIdentificacion = $"{index:00000000}",
            FechaNacimiento = DateOnly.FromDateTime(faker.Date.Between(
                DateTime.UtcNow.AddYears(-70), DateTime.UtcNow.AddYears(-18))),
            Telefono = faker.Phone.PhoneNumber(),
            Correo = faker.Internet.Email(),
            Estado = true
        });

        await _context.Clientes.AddRangeAsync(clientes);
        await _context.SaveChangesAsync();
    }
}