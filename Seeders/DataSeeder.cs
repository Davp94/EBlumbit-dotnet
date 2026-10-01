using System;

namespace EBlumbit.Seeders;

public class DataSeeder(PermisosSeeder permisosSeeder, RolSeeder rolSeeder, UsuarioSeeder usuarioSeeder)
{

    private readonly PermisosSeeder _permisosSeeder = permisosSeeder;
    private readonly RolSeeder _rolSeeder = rolSeeder;
    private readonly UsuarioSeeder _usuarioSeeder = usuarioSeeder;

    public async Task SeedAsync()
    {
        //execute seeders
        await _permisosSeeder.SeedAsync();
        await _rolSeeder.SeedAsync();
        await _usuarioSeeder.SeedAsync();
        
    }
}
