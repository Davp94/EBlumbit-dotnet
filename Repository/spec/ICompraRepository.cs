using EBlumbit.Models;

namespace EBlumbit.Repository.spec;

public interface ICompraRepository
{
    Task<IEnumerable<Compra>> GetAllCompras();
    Task<Compra?> GetCompraById(int id);
    Task<Compra> CreateCompra(Compra compra);
    Task UpdateCompra(Compra compra);
    Task<int> GetNextCodigoSeq();
}