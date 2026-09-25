using System;
using EBlumbit.Dto.Ventas;
using EBlumbit.Models;

namespace EBlumbit.Builders;

public class VentasBuilder
{
    public static Venta ToEntity(CreateVentaRequest request, string codigo)
    {
        return new Venta
        {
            Codigo = codigo,
            Fecha = DateTime.UtcNow,
            DescuentoTotal = request.DescuentoTotal,
            Observacion = request.Observacion,
            ClienteId = request.ClienteId,
            UsuarioId = request.UsuarioId,
            Estado = true,
            DetalleVentas = request.DetalleVenta.Select(d => new DetalleVenta
            {
                Cantidad = d.Cantidad,
                PrecionUnitarioVenta = d.PrecioUnitarioVenta,
                Observaciones = d.Observaciones,
                ProductoId = d.ProductoId,
                AlmacenId = d.AlmacenId
            }).ToList()
        };
    }

    public static VentasResponse ToResponseDto(Venta venta)
    {
        return new VentasResponse
        {
            Id = venta.Id,
            Codigo = venta.Codigo,
            Fecha = venta.Fecha.ToString("dd-MM-yyy HH:mm:ss"),
            DescuentoTotal = venta.DescuentoTotal,
            Observacion = venta.Observacion,
            UsuarioId = venta.UsuarioId,
            Username = venta.Users.Name,
            ClienteId = venta.ClienteId,
            NroIdentificacionCliente = venta.cliente.NroIdentificacion
        };
    }

    public static VentasDetailResponse ToDetailResponseDto(Venta venta)
    {
        return new VentasDetailResponse
        {
            Id = venta.Id,
            Codigo = venta.Codigo,
            Fecha = venta.Fecha.ToString("dd-MM-yyy HH:mm:ss"),
            DescuentoTotal = venta.DescuentoTotal,
            Observacion = venta.Observacion,
            UsuarioId = venta.UsuarioId,
            Username = venta.Users.Name,
            ClienteId = venta.ClienteId,
            NroIdentificacionCliente = venta.cliente.NroIdentificacion,
            DetalleVenta = venta.DetalleVentas.Select(ToDetalleVentaResponseDto).ToList()
        };
    }

    public static DetalleVentaResponse ToDetalleVentaResponseDto(DetalleVenta detalle)
    {
        return new DetalleVentaResponse
        {
            Id = detalle.Id,
            Cantidad = detalle.Cantidad,
            PrecioUnitarioVenta = detalle.PrecionUnitarioVenta,
            Observaciones = detalle.Observaciones,
            ProductoId = detalle.ProductoId,
            ProductoNombre = detalle.Producto.Nombre,
            VentaId = detalle.VentaId,
            AlmacenId = detalle.AlmacenId,
            AlmacenNombre = detalle.Almacen.Nombre
        };
    }
}
