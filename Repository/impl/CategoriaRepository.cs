using System;
using EBlumbit.Data;
using EBlumbit.Models;
using EBlumbit.Repository.spec;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Repository.impl;

public class CategoriaRepository(AppDbContext context) : ICategoriaRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Categoria>> GetAllCategorias()
    {
        return await _context.Categorias.ToListAsync();
    }

    public async Task<Categoria?> GetCategoriaById(int id)
    {
        return await _context.Categorias.FirstOrDefaultAsync(c=>c.Id == id);
    }

    public async Task<Categoria?> CreateCategoria(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task<Categoria?> UpdateCategorias(Categoria categoria)
    {
         _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task DeleteCategoria(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if(categoria != null)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
