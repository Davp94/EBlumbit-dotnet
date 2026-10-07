using System;
using EBlumbit.Data;
using EBlumbit.Dto.Productos;
using EBlumbit.Models;
using EBlumbit.Repository.spec;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Repository.impl;

public class ProductoRepository(AppDbContext context) : IProductoRepository
{
    private readonly AppDbContext _context = context;

    public Task<(IEnumerable<Productos> Items, int TotalCount)>  GetProductosPagination(ProductoQueryParams queryParams)
    {
        throw new NotImplementedException();
    }
    public async Task<Productos> GetProductoById(int id)
    {
        return await _context.Productos.Include(p=>p.Categoria)
        .FirstOrDefaultAsync(p=>p.Id == id);
    }

 
    public async Task<Productos> CreateProducto(Productos producto)
    {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        return producto;
    }

    public async Task<Productos> UpdateProducto(Productos producto)
    {
        _context.Productos.Update(producto);
        await _context.SaveChangesAsync();
        return producto;
    }
    public async Task DeleteProducto(int id)
    {
       var producto = await _context.Productos.FindAsync(id);
       if(producto != null)
        {
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
        }
    }

}
