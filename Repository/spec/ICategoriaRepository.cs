using System;
using EBlumbit.Models;

namespace EBlumbit.Repository.spec;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllCategorias();
    Task<Categoria?> GetCategoriaById(int id);
    Task<Categoria?> CreateCategoria(Categoria categoria);
    Task<Categoria?> UpdateCategorias(Categoria categoria);
    Task DeleteCategoria(int id);
}
