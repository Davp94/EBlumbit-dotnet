using System;

namespace EBlumbit.Seeders;

public class DataSeeder(
    PermisosSeeder permisosSeeder,
    RolSeeder rolSeeder,
    UsuarioSeeder usuarioSeeder,
    CategoriaSeeder categoriaSeeder,
    SucursalSeeder sucursalSeeder,
    AlmacenSeeder almacenSeeder,
    ProductoSeeder productoSeeder,
    InventarioSeeder inventarioSeeder,
    ClienteSeeder clienteSeeder,
    ProveedorSeeder proveedorSeeder)
{

    private readonly PermisosSeeder _permisosSeeder = permisosSeeder;
    private readonly RolSeeder _rolSeeder = rolSeeder;
    private readonly UsuarioSeeder _usuarioSeeder = usuarioSeeder;
    private readonly CategoriaSeeder _categoriaSeeder = categoriaSeeder;
    private readonly SucursalSeeder _sucursalSeeder = sucursalSeeder;
    private readonly AlmacenSeeder _almacenSeeder = almacenSeeder;
    private readonly ProductoSeeder _productoSeeder = productoSeeder;
    private readonly InventarioSeeder _inventarioSeeder = inventarioSeeder;
    private readonly ClienteSeeder _clienteSeeder = clienteSeeder;
    private readonly ProveedorSeeder _proveedorSeeder = proveedorSeeder;

    public async Task SeedAsync()
    {
        //execute seeders
        await _permisosSeeder.SeedAsync();
        await _rolSeeder.SeedAsync();
        await _usuarioSeeder.SeedAsync();
        await _categoriaSeeder.SeedAsync();
        await _sucursalSeeder.SeedAsync();
        await _almacenSeeder.SeedAsync();
        await _productoSeeder.SeedAsync();
        await _inventarioSeeder.SeedAsync();
        await _clienteSeeder.SeedAsync();
        await _proveedorSeeder.SeedAsync();
    }
}
