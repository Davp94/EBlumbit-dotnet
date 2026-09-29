using EBlumbit.Dto.Compras;
using EBlumbit.Models;

namespace EBlumbit.Builders;

public static class ComprasBuilder
{
    public static Compra ToEntity(CreateCompraRequest request, string codigo)
    {
        return new Compra
        {
            Codigo = codigo,
            Fecha = DateTime.UtcNow,
            ProveedorId = request.ProveedorId,
            UsuarioId = request.UsuarioId,
            DescuentoTotal = request.DescuentoTotal,
            Estado = "RECIBIDA",
            Detalle = request.Detalle,
            Observaciones = request.Observaciones,
            DetalleCompras = request.DetalleCompra.Select(d => new DetalleCompra
            {
                ProductoId = d.ProductoId,
                AlmacenId = d.AlmacenId,
                Cantidad = d.Cantidad,
                PrecioUnitarioCompra = d.PrecioUnitarioCompra,
                Observaciones = d.Observaciones
            }).ToList()
        };
    }

    public static CompraResponse ToResponseDto(Compra compra)
    {
        return new CompraResponse
        {
            Id = compra.Id,
            Codigo = compra.Codigo,
            Fecha = compra.Fecha,
            ProveedorId = compra.ProveedorId,
            ProveedorRazonSocial = compra.Proveedor.RazonSocial,
            ProveedorNroIdentificacion = compra.Proveedor.NroIdentificacion,
            UsuarioId = compra.UsuarioId,
            Username = compra.Usuario.Name,
            DescuentoTotal = compra.DescuentoTotal,
            Estado = compra.Estado,
            Detalle = compra.Detalle,
            Observaciones = compra.Observaciones
        };
    }

    public static CompraDetailResponse ToDetailResponseDto(Compra compra)
    {
        var response = new CompraDetailResponse
        {
            DetalleCompra = compra.DetalleCompras.Select(ToDetalleResponseDto).ToList()
        };
        var basicResponse = ToResponseDto(compra);
        response.Id = basicResponse.Id;
        response.Codigo = basicResponse.Codigo;
        response.Fecha = basicResponse.Fecha;
        response.ProveedorId = basicResponse.ProveedorId;
        response.ProveedorRazonSocial = basicResponse.ProveedorRazonSocial;
        response.ProveedorNroIdentificacion = basicResponse.ProveedorNroIdentificacion;
        response.UsuarioId = basicResponse.UsuarioId;
        response.Username = basicResponse.Username;
        response.DescuentoTotal = basicResponse.DescuentoTotal;
        response.Estado = basicResponse.Estado;
        response.Detalle = basicResponse.Detalle;
        response.Observaciones = basicResponse.Observaciones;
        return response;
    }

    private static DetalleCompraResponse ToDetalleResponseDto(DetalleCompra detalle)
    {
        return new DetalleCompraResponse
        {
            Id = detalle.Id,
            CompraId = detalle.CompraId,
            ProductoId = detalle.ProductoId,
            ProductoNombre = detalle.Producto.Nombre,
            AlmacenId = detalle.AlmacenId,
            AlmacenNombre = detalle.Almacen.Nombre,
            Cantidad = detalle.Cantidad,
            PrecioUnitarioCompra = detalle.PrecioUnitarioCompra,
            Observaciones = detalle.Observaciones
        };
    }
}