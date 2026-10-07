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

    public async Task<(IEnumerable<Productos> Items, int TotalCount)>  GetProductosPagination(ProductoQueryParams queryParams)
    {
        IQueryable<Productos> query = _context.Productos
        .Include(p=>p.Categoria).AsNoTracking();
        //filtering
        if(!string.IsNullOrWhiteSpace(queryParams.Search))
        {
            var search = queryParams.Search.Trim().ToLower();
            query = query.Where(p=>
                (p.Nombre != null && p.Nombre.ToLower().Contains(search)) ||
                (p.Descripcion != null && p.Descripcion.ToLower().Contains(search)) || (p.Marca != null && p.Marca.ToLower().Contains(search)) || (p.CodigoBarra != null && p.CodigoBarra.ToLower().Contains(search))
            );
        }
        if(queryParams.CategoriaId.HasValue)
        {
           query = query.Where(p=>p.CategoriaId == queryParams.CategoriaId); 
        }
        if(queryParams.Estado.HasValue)
        {
            query = query.Where(p=>p.Estado == queryParams.Estado);
        }
        if(!string.IsNullOrWhiteSpace(queryParams.Marca))
        {
            var marca = queryParams.Marca.Trim().ToLower();
            query = query.Where(p=>p.Marca.ToLower().Contains(marca));
        }
        //Sorting
        query = (queryParams.SortBy.Trim().ToLower(), queryParams.IsAscending) switch
        {
            ("nombre", true) => query.OrderBy(p=>p.Nombre),
            ("nombre", false) => query.OrderByDescending(p=>p.Nombre),
            ("precio", true) => query.OrderBy(p=>p.PrecioVentaActual),
            ("precio", false) => query.OrderByDescending(p=>p.PrecioVentaActual),
            ("fecha_registro", true) => query.OrderBy(p=>p.FechaRegistro),
            ("fecha_registro", false) => query.OrderByDescending(p=>p.FechaRegistro),
            _ => query.OrderBy(p=>p.Id)
        };
        //pagination
        var TotalCount = await query.CountAsync();
        var items = await query.Skip((queryParams.PageNumber -1 )*queryParams.PageSize).Take(queryParams.PageSize)
        .ToListAsync();
        return (items, TotalCount);
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
