using System.Globalization;
using EBlumbit.Dto.Clientes;
using EBlumbit.Models;

namespace EBlumbit.Builders;

public static class ClientesBuilder
{
    private const string DateFormat = "yyyy-MM-dd";

    public static Cliente ToEntity(ClienteRequestDto requestDto)
    {
        return new Cliente
        {
            NombreCompleto = requestDto.NombreCompleto,
            NroIdentificacion = requestDto.NroIdentificacion,
            FechaNacimiento = DateOnly.ParseExact(requestDto.FechaNacimiento, DateFormat, CultureInfo.InvariantCulture),
            Telefono = requestDto.Telefono,
            Correo = requestDto.Correo,
            Estado = true
        };
    }

    public static ClienteResponseDto ToResponseDto(Cliente cliente)
    {
        return new ClienteResponseDto
        {
            Id = cliente.Id,
            NombreCompleto = cliente.NombreCompleto,
            NroIdentificacion = cliente.NroIdentificacion,
            FechaNacimiento = cliente.FechaNacimiento.ToString(DateFormat, CultureInfo.InvariantCulture),
            Telefono = cliente.Telefono,
            Correo = cliente.Correo,
            Estado = cliente.Estado
        };
    }

    public static Cliente ToEntityUpdate(ClienteRequestDto requestDto, Cliente cliente)
    {
        cliente.NombreCompleto = requestDto.NombreCompleto;
        cliente.NroIdentificacion = requestDto.NroIdentificacion;
        cliente.FechaNacimiento = DateOnly.ParseExact(requestDto.FechaNacimiento, DateFormat, CultureInfo.InvariantCulture);
        cliente.Telefono = requestDto.Telefono;
        cliente.Correo = requestDto.Correo;

        return cliente;
    }
}