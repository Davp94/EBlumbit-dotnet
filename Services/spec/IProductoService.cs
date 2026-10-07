using System;
using EBlumbit.Dto.Common;
using EBlumbit.Dto.Productos;

namespace EBlumbit.Services.spec;

public interface IProductoService
{
    Task<PageResult<ProductoResponseDto>> GetProductosPagination(ProductoQueryParams queryParams);

    Task<ProductoResponseDto> GetProductoById(int id);
    Task<ProductoResponseDto> CreateProducto(ProductoRequestDto requestDto);
    Task<ProductoResponseDto> UpdateProducto(int id, ProductoRequestDto requestDto);
    Task DeleteProducto(int id);
}
