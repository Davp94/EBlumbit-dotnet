using System;
using EBlumbit.Data;
using EBlumbit.Models;
using EBlumbit.Repository.spec;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Repository.impl;

public class VentaRepository(AppDbContext appDbContext) : IVentaRepository
{

    private readonly AppDbContext _context = appDbContext;

   public async Task<IEnumerable<Venta>> GetAllVentas()
    {
        return await _context.Ventas
            .Include(v => v.Users)
            .Include(v => v.cliente)
            .ToListAsync();
    }

    public async Task<Venta?> GetVentaById(int id)
    {
        return await _context.Ventas
            .Include(v => v.Users)
            .Include(v => v.cliente)
            .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Producto)
            .Include(v => v.DetalleVentas)
                .ThenInclude(d => d.Almacen)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Venta> CreateVenta(Venta venta)
    {
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();
        return venta;
    }

    public async Task UpdateVenta(Venta venta)
    {
        _context.Ventas.Update(venta);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetNextCodigoSeq()
    {
        var count = await _context.Ventas.CountAsync();
        return count + 1;
    }
}
