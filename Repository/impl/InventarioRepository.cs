using System;
using EBlumbit.Data;
using EBlumbit.Models;
using EBlumbit.Repository.spec;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Repository.impl;

public class InventarioRepository(AppDbContext appDbContext) : IInventarioRepository
{
    private readonly AppDbContext _context = appDbContext;

    public async Task<Inventario> GetByProductoAndAlmacen(int productoId, int almacenId)
    {
        return await _context.Inventarios.FirstOrDefaultAsync(i => i.ProductoId == productoId && i.AlmacenId == almacenId);
    }

    public async Task UpdateInventario(Inventario inventario)
    {
        _context.Inventarios.Update(inventario);
        await _context.SaveChangesAsync();
    }
}
