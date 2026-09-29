using System;
using EBlumbit.Models;

namespace EBlumbit.Repository.spec;

public interface IInventarioRepository
{
    Task<Inventario?> GetByProductoAndAlmacen(int productoId, int almacenId);
    Task CreateInventario(Inventario inventario);
    Task UpdateInventario(Inventario inventario);

}
