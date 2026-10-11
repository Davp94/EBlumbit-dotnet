using System;
using Dapper;
using EBlumbit.Models;
using EBlumbit.Repository.spec;
using Npgsql;

namespace EBlumbit.Repository.impl;

public class ClienteDapperRepository(IConfiguration configuration) : IClienteRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");
    private NpgsqlConnection CreateConnection() => new (_connectionString);
    
    public async Task<IEnumerable<Cliente>> GetAllClientes()
    {
        using var connection = CreateConnection();
        const string sql ="""
            select "Id", "NombreCompleto", "NroIdentificacion", "FechaNacmiento", "Telefono", "Correo", "Estado" 
            from "Clientes"
            order by "Id"
        """;
        return await connection.QueryAsync<Cliente>(sql);
    }

    public async Task<Cliente?> GetClienteById(int id)
    {
        using var connection = CreateConnection();
        const string sql ="""
            select "Id", "NombreCompleto", "NroIdentificacion", "FechaNacmiento", "Telefono", "Correo", "Estado" 
            from "Clientes"
            where "Id" = @Id
        """;
        return await connection.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id});
    }
    public async Task<Cliente> CreateCliente(Cliente cliente)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT * FROM sp_crear_cliente(@p_nombre_completo, @p_nro_identificacion, @p_fecha_nacimiento, @p_telefono, @p_correo)
        """;
        var result = await connection.QueryFirstAsync<Cliente>(sql, new
        {
            p_nombre_completo = cliente.NombreCompleto,
            p_nro_identificacion = cliente.NroIdentificacion,
            p_fecha_nacimiento = cliente.FechaNacimiento,
            p_telefono = cliente.Telefono,
            p_correo = cliente.Correo
        });
        return result;
    }

    public async Task UpdateCliente(Cliente cliente)
    {
        using var connection = CreateConnection();
        const string sql = """
            UPDATE "Clientes"
            set "NombreCompleto" = @NombreCompleto,
                "NroIdentificacion" = @NroIdentificacion,
                "FechaNacimiento" = @FechaNacimiento,
                "Telefono" = @Telefono,
                "Correo" = @Correo,
                "Estado" = @Estado
            Where "Id" = @Id
        """;
        await connection.ExecuteAsync(sql, cliente);
    }

    public async Task DeleteCliente(int id)
    {
        using var connection = CreateConnection();
        const string sql = """
            delete from "Clientes"
            WHERE "Id" = @Id
        """;
        await connection.ExecuteAsync(sql, new {Id = id});
    }


}
