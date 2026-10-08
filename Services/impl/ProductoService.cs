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
            throw new ResourceNotFoundException("producto no encontrado");
        }
        return ProductosBuilder.ToResponseDto(producto);
    }


    public async Task<ProductoResponseDto> CreateProducto(ProductoRequestDto requestDto)
    {
        var imageProductoPath = await _fileService.SaveFile(requestDto.Imagen);
        var productoToCreated = ProductosBuilder.ToEntity(requestDto);
        productoToCreated.Imagen = imageProductoPath;
        var created = await _productoRepository.CreateProducto(productoToCreated);
        return ProductosBuilder.ToResponseDto(created);
    }

    

    public async Task<ProductoResponseDto> UpdateProducto(int id, ProductoRequestDto requestDto)
    {
       var producto = await _productoRepository.GetProductoById(id);
       if(producto == null)
        {
            throw new ResourceNotFoundException("producto no encontrado");
        }
       var entityToUpdate = ProductosBuilder.ToEntityUpdate(requestDto, id);
       entityToUpdate.FechaRegistro = producto.FechaRegistro;

       var updated = await _productoRepository.UpdateProducto(entityToUpdate);
       var result = await _productoRepository.GetProductoById(id);
       return ProductosBuilder.ToResponseDto(result);
    }

    public async Task DeleteProducto(int id)
    {
        var producto = await _productoRepository.GetProductoById(id);
        if(producto == null)
        {
            throw new ResourceNotFoundException("producto no encontrado");
        }
        await _productoRepository.DeleteProducto(id);
    }
}
