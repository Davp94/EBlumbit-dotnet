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

    private const string SelectColumns =
        "\"Id\", \"NombreCompleto\", \"NroIdentificacion\", \"FechaNacimiento\", \"Telefono\", \"Correo\", \"Estado\"";

    public async Task<IEnumerable<Cliente>> GetAllClientes()
    {
        using var connection = CreateConnection();
        var sql = $"""
            select {SelectColumns}
            from "Clientes"
            order by "Id"
        """;
        return await connection.QueryAsync<Cliente>(sql);
    }

    public async Task<Cliente?> GetClienteById(int id)
    {
        using var connection = CreateConnection();
        var sql = $"""
            select {SelectColumns}
            from "Clientes"
            where "Id" = @Id
        """;
        return await connection.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id});
    }

    public async Task<Cliente> CreateCliente(Cliente cliente)
    {
        using var connection = CreateConnection();
        var sql = $"""
            insert into "Clientes"
                ("NombreCompleto", "NroIdentificacion", "FechaNacimiento", "Telefono", "Correo", "Estado")
            values
                (@NombreCompleto, @NroIdentificacion, @FechaNacimiento, @Telefono, @Correo, @Estado)
            returning {SelectColumns}
        """;
        return await connection.QueryFirstAsync<Cliente>(sql, new
        {
            cliente.NombreCompleto,
            cliente.NroIdentificacion,
            cliente.FechaNacimiento,
            cliente.Telefono,
            cliente.Correo,
            cliente.Estado
        });
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
