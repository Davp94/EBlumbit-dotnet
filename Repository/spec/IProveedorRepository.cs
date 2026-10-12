using EBlumbit.Models;

namespace EBlumbit.Repository.spec;

public interface IProveedorRepository
{
    Task<IEnumerable<Proveedor>> GetAllProveedores();
    Task<Proveedor?> GetProveedorById(int id);
    Task<Proveedor> CreateProveedor(Proveedor proveedor);
    Task UpdateProveedor(Proveedor proveedor);
    Task DeleteProveedor(int id);
}