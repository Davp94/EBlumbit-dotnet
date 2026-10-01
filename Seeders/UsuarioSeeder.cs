using System;
using Bogus;
using EBlumbit.Data;

namespace EBlumbit.Seeders;

public class UsuarioSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if(_context.Users.Any())
            return;
        var faker = new Faker("es");
        var usuarios = new List<Users>();

        for(int i = 0; i < 10; i++)
        {
            var username = faker.Internet.UserName();
            var usuario = new Users
            {
                Email = faker.Internet.Email(username),
                Name = username,
                Password = "Password123*"
            }; 
            usuarios.Add(usuario);
        }

        await _context.Users.AddRangeAsync(usuarios);
        await _context.SaveChangesAsync();
    }   
}
