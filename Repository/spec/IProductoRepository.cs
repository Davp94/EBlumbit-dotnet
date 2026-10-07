using System;
using EBlumbit.Dto.Productos;
using EBlumbit.Models;

namespace EBlumbit.Repository.spec;

public interface IProductoRepository
{
    Task<(IEnumerable<Productos> Items, int TotalCount)> GetProductosPagination(ProductoQueryParams queryParams);
    Task<Productos> GetProductoById(int id);
    Task<Productos> CreateProducto(Productos producto);
    Task<Productos> UpdateProducto(Productos producto);
    Task DeleteProducto(int id);
}
