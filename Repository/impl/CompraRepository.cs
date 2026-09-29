using EBlumbit.Data;
using EBlumbit.Models;
using EBlumbit.Repository.spec;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Repository.impl;

public class CompraRepository(AppDbContext appDbContext) : ICompraRepository
{
    private readonly AppDbContext _context = appDbContext;

    public async Task<IEnumerable<Compra>> GetAllCompras()
    {
        return await _context.Compras
            .Include(c => c.Proveedor)
            .Include(c => c.Usuario)
            .ToListAsync();
    }

    public async Task<Compra?> GetCompraById(int id)
    {
        return await _context.Compras
            .Include(c => c.Proveedor)
            .Include(c => c.Usuario)
            .Include(c => c.DetalleCompras).ThenInclude(d => d.Producto)
            .Include(c => c.DetalleCompras).ThenInclude(d => d.Almacen)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Compra> CreateCompra(Compra compra)
    {
        _context.Compras.Add(compra);
        await _context.SaveChangesAsync();
        return await _context.Compras
            .Include(c => c.Proveedor)
            .Include(c => c.Usuario)
            .SingleAsync(c => c.Id == compra.Id);
    }

    public async Task UpdateCompra(Compra compra)
    {
        _context.Compras.Update(compra);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetNextCodigoSeq()
    {
        return await _context.Compras.CountAsync() + 1;
    }
}