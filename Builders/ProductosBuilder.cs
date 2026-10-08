using System;
using EBlumbit.Dto.Productos;
using EBlumbit.Models;

namespace EBlumbit.Builders;

public static class ProductosBuilder
{
    public static ProductoResponseDto ToResponseDto(Productos producto)
    {
        return new ProductoResponseDto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            CodigoBarra = producto.CodigoBarra,
            UnidadMedida = producto.UnidadMedida,
            Marca = producto.Marca,
            Imagen = producto.Imagen,
            Descripcion = producto.Descripcion,
            PrecioVentaActual = producto.PrecioVentaActual,
            StockMinimo = producto.StockMinimo,
            Estado = producto.Estado,
            FechaRegistro = producto.FechaRegistro.ToString("O"),
            CategoriaId = producto.CategoriaId,
            CategoriaNombre = producto.Categoria?.Nombre
        };
    }

    public static Productos ToEntity(ProductoRequestDto productoRequest)
    {
        return new Productos
        {
            Nombre = productoRequest.Nombre,
            CodigoBarra = productoRequest.CodigoBarra,
            UnidadMedida = productoRequest.UnidadMedida,
            Marca = productoRequest.Marca,
            Imagen = productoRequest.Imagen,
            Descripcion = productoRequest.Descripcion,
            PrecioVentaActual = productoRequest.PrecioVentaActual,
            StockMinimo = productoRequest.StockMinimo,
            Estado = productoRequest.Estado,
            CategoriaId = productoRequest.CategoriaId
        };
    }

    public static Productos ToEntityUpdate(ProductoRequestDto productoRequestDto, int id)
    {
        var producto = ToEntity(productoRequestDto);
        producto.Id = id;
        return producto;
    }
}
