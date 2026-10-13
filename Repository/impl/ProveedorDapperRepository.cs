using Dapper;
using EBlumbit.Models;
using EBlumbit.Repository.spec;
using Npgsql;

namespace EBlumbit.Repository.impl;

public class ProveedorDapperRepository(IConfiguration configuration) : IProveedorRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");
    private NpgsqlConnection CreateConnection() => new(_connectionString);

    private const string SelectColumns = "id AS \"Id\", razon_social AS \"RazonSocial\", nro_identificacion AS \"NroIdentificacion\", contacto AS \"Contacto\", telefono AS \"Telefono\", correo AS \"Correo\", observaciones AS \"Observaciones\", estado AS \"Estado\"";

    public async Task<IEnumerable<Proveedor>> GetAllProveedores()
    {
        using var connection = CreateConnection();
        var sql = $"SELECT {SelectColumns} FROM proveedores ORDER BY id";
        return await connection.QueryAsync<Proveedor>(sql);
    }

    public async Task<Proveedor?> GetProveedorById(int id)
    {
        using var connection = CreateConnection();
        var sql = $"SELECT {SelectColumns} FROM proveedores WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<Proveedor>(sql, new { Id = id });
    }

    public async Task<Proveedor> CreateProveedor(Proveedor proveedor)
    {
        using var connection = CreateConnection();
        const string sql = """
            INSERT INTO proveedores (razon_social, nro_identificacion, contacto, telefono, correo, observaciones, estado)
            VALUES (@RazonSocial, @NroIdentificacion, @Contacto, @Telefono, @Correo, @Observaciones, @Estado)
            RETURNING id AS "Id", razon_social AS "RazonSocial", nro_identificacion AS "NroIdentificacion", contacto AS "Contacto", telefono AS "Telefono", correo AS "Correo", observaciones AS "Observaciones", estado AS "Estado"
            """;
        return await connection.QuerySingleAsync<Proveedor>(sql, proveedor);
    }

    public async Task UpdateProveedor(Proveedor proveedor)
    {
        using var connection = CreateConnection();
        const string sql = """
            UPDATE proveedores
            SET razon_social = @RazonSocial,
                nro_identificacion = @NroIdentificacion,
                contacto = @Contacto,
                telefono = @Telefono,
                correo = @Correo,
                observaciones = @Observaciones,
                estado = @Estado
            WHERE id = @Id
            """;
        await connection.ExecuteAsync(sql, proveedor);
    }

    public async Task DeleteProveedor(int id)
    {
        using var connection = CreateConnection();
        const string sql = "DELETE FROM proveedores WHERE id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}