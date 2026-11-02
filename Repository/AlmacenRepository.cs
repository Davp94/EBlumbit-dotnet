using EBlumbit.Data;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Repository;

public class AlmacenRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Almacenes>> GetAllAsync()
    {
        return await _context.Almacenes
            .Include(a => a.Sucursal)
            .OrderBy(a => a.Id)
            .ToListAsync();
    }

    public async Task<Almacenes?> GetByIdAsync(int id)
    {
        return await _context.Almacenes
            .Include(a => a.Sucursal)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Almacenes> CreateAsync(Almacenes almacen)
    {
        _context.Almacenes.Add(almacen);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(almacen.Id))!;
    }

    public async Task<Almacenes> UpdateAsync(Almacenes almacen)
    {
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(almacen.Id))!;
    }

    public async Task DeleteAsync(int id)
    {
        var almacen = await _context.Almacenes.FindAsync(id);
        if (almacen != null)
        {
            _context.Almacenes.Remove(almacen);
            await _context.SaveChangesAsync();
        }
    }
}
