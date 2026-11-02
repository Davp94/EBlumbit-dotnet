using System;
using EBlumbit.Builders;
using EBlumbit.Dto.Common;
using EBlumbit.Dto.Productos;
using EBlumbit.exceptions;
using EBlumbit.Repository.spec;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class ProductoService(IProductoRepository productoRepository, IFIleService fileService) : IProductoService
{
    private readonly IProductoRepository _productoRepository = productoRepository;

    private readonly IFIleService _fileService = fileService;

    public async Task<PageResult<ProductoResponseDto>> GetProductosPagination(ProductoQueryParams queryParams)
    {
        var (items, totalCount) = await _productoRepository.GetProductosPagination(queryParams);

        return new PageResult<ProductoResponseDto>
        {
            Items = items.Select(ProductosBuilder.ToResponseDto),
            TotalCount = totalCount,
            PageNumber = queryParams.PageNumber,
            PageSize = queryParams.PageSize
        };
    }

    public async Task<ProductoResponseDto> GetProductoById(int id)
    {
        var producto = await _productoRepository.GetProductoById(id);
        if(producto == null)
        {
            throw new ResourceNotFoundException($"Producto con id {id} no encontrado");
        }
        return ProductosBuilder.ToResponseDto(producto);
    }


    public async Task<ProductoResponseDto> CreateProducto(ProductoRequestDto requestDto)
    {
        var imageProductoPath = await _fileService.SaveFile(requestDto.Imagen);
        var productoToCreated = ProductosBuilder.ToEntity(requestDto);
        productoToCreated.Imagen = imageProductoPath;
        // Se trunca a microsegundos para coincidir con la precision de Postgres
        // y que create y update devuelvan el mismo fechaRegistro.
        var now = DateTime.UtcNow;
        productoToCreated.FechaRegistro = now.AddTicks(-(now.Ticks % 10));
        var created = await _productoRepository.CreateProducto(productoToCreated);
        // Se recarga con la categoria para que el response incluya categoriaNombre.
        var reloaded = await _productoRepository.GetProductoById(created.Id);
        return ProductosBuilder.ToResponseDto(reloaded);
    }

    

    public async Task<ProductoResponseDto> UpdateProducto(int id, ProductoRequestDto requestDto)
    {
       // Se actualiza la entidad rastreada para evitar conflictos de tracking en EF.
       var producto = await _productoRepository.GetProductoById(id);
       if(producto == null)
        {
            throw new ResourceNotFoundException($"Producto con id {id} no encontrado");
        }

       producto.Nombre = requestDto.Nombre;
       producto.CodigoBarra = requestDto.CodigoBarra;
       producto.UnidadMedida = requestDto.UnidadMedida;
       producto.Marca = requestDto.Marca;
       producto.Descripcion = requestDto.Descripcion;
       producto.PrecioVentaActual = requestDto.PrecioVentaActual;
       producto.StockMinimo = requestDto.StockMinimo;
       producto.Estado = requestDto.Estado;
       producto.CategoriaId = requestDto.CategoriaId;

       var updated = await _productoRepository.UpdateProducto(producto);
       return ProductosBuilder.ToResponseDto(updated);
    }

    public async Task DeleteProducto(int id)
    {
        var producto = await _productoRepository.GetProductoById(id);
        if(producto == null)
        {
            throw new ResourceNotFoundException($"Producto con id {id} no encontrado");
        }
        await _productoRepository.DeleteProducto(id);
    }
}
