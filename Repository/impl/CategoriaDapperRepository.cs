using System;
using EBlumbit.Models;
using EBlumbit.Repository.spec;

namespace EBlumbit.Repository.impl;

public class CategoriaDapperRepository : ICategoriaRepository
{
    public Task<Categoria?> CreateCategoria(Categoria categoria)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategoria(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Categoria>> GetAllCategorias()
    {
        throw new NotImplementedException();
    }

    public Task<Categoria?> GetCategoriaById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Categoria?> UpdateCategorias(Categoria categoria)
    {
        throw new NotImplementedException();
    }
}
