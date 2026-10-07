using System;
using EBlumbit.Dto.Common;
using EBlumbit.Dto.Productos;
using EBlumbit.Repository.spec;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class ProductoService(IProductoRepository productoRepository) : IProductoService
{
    private readonly IProductoRepository _productoRepository = productoRepository;
    public Task<ProductoResponseDto> CreateProducto(ProductoRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProducto(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductoResponseDto> GetProductoById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<PageResult<ProductoResponseDto>> GetProductosPagination(ProductoQueryParams queryParams)
    {
        throw new NotImplementedException();
    }

    public Task<ProductoResponseDto> UpdateProducto(int id, ProductoRequestDto requestDto)
    {
        throw new NotImplementedException();
    }
}
