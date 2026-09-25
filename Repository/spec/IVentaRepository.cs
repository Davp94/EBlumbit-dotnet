using System;
using EBlumbit.Models;

namespace EBlumbit.Repository.spec;

public interface IVentaRepository
{
    Task<IEnumerable<Venta>> GetAllVentas();
    Task<Venta> GetVentaById(int id);
    Task<Venta> CreateVenta(Venta venta);
    Task UpdateVenta(Venta venta);
    Task<int> GetNextCodigoSeq();
}
