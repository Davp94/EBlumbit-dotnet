using EBlumbit.Dto.Proveedores;
using EBlumbit.Models;

namespace EBlumbit.Builders;

public static class ProveedoresBuilder
{
    public static Proveedor ToEntity(ProveedorRequestDto requestDto)
    {
        return new Proveedor
        {
            RazonSocial = requestDto.RazonSocial,
            NroIdentificacion = requestDto.NroIdentificacion,
            Contacto = requestDto.Contacto,
            Telefono = requestDto.Telefono,
            Correo = requestDto.Correo,
            Observaciones = requestDto.Observaciones,
            Estado = true
        };
    }

    public static ProveedorResponseDto ToResponseDto(Proveedor proveedor)
    {
        return new ProveedorResponseDto
        {
            Id = proveedor.Id,
            RazonSocial = proveedor.RazonSocial,
            NroIdentificacion = proveedor.NroIdentificacion,
            Contacto = proveedor.Contacto,
            Telefono = proveedor.Telefono,
            Correo = proveedor.Correo,
            Observaciones = proveedor.Observaciones,
            Estado = proveedor.Estado
        };
    }

    public static Proveedor ToEntityUpdate(ProveedorRequestDto requestDto, Proveedor proveedor)
    {
        proveedor.RazonSocial = requestDto.RazonSocial;
        proveedor.NroIdentificacion = requestDto.NroIdentificacion;
        proveedor.Contacto = requestDto.Contacto;
        proveedor.Telefono = requestDto.Telefono;
        proveedor.Correo = requestDto.Correo;
        proveedor.Observaciones = requestDto.Observaciones;
        return proveedor;
    }
}