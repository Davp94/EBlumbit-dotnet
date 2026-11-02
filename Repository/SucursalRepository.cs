using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Repository;

public class SucursalRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Sucursales>> GetAllAsync()
    {
        return await _context.Sucursales.OrderBy(s => s.Id).ToListAsync();
    }

    public async Task<Sucursales?> GetByIdAsync(int id)
    {
        return await _context.Sucursales.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Sucursales> CreateAsync(Sucursales sucursal)
    {
        _context.Sucursales.Add(sucursal);
        await _context.SaveChangesAsync();
        return sucursal;
    }

    public async Task<Sucursales> UpdateAsync(Sucursales sucursal)
    {
        _context.Sucursales.Update(sucursal);
        await _context.SaveChangesAsync();
        return sucursal;
    }

    public async Task DeleteAsync(int id)
    {
        var sucursal = await _context.Sucursales.FindAsync(id);
        if (sucursal != null)
        {
            _context.Sucursales.Remove(sucursal);
            await _context.SaveChangesAsync();
        }
    }
}
