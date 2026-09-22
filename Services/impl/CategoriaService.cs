using System;
using EBlumbit.Dto;
using EBlumbit.Repository.spec;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class CategoriaService(ICategoriaRepository categoriaRepository) : ICategoriaService
{

    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
    public Task<IEnumerable<CategoriaResponseDto>> GetAllCategorias()
    {
        throw new NotImplementedException();
    }

    public Task<CategoriaResponseDto> GetCategoriasById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<CategoriaResponseDto> CreateCategoria(CreateCategoriaDto createCategoriaDto)
    {
        throw new NotImplementedException();
    }
    public Task<CategoriaResponseDto> UpdateCategoria(int id, CreateCategoriaDto createCategoriaDto)
    {
        throw new NotImplementedException();
    }
    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }



   
}
