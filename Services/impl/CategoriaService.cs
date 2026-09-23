using System;
using EBlumbit.Builders;
using EBlumbit.Dto;
using EBlumbit.Repository.spec;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class CategoriaService(ICategoriaRepository categoriaRepository) : ICategoriaService
{

    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
    public async Task<IEnumerable<CategoriaResponseDto>> GetAllCategorias()
    {
        var categorias = await _categoriaRepository.GetAllCategorias();
        return categorias.Select(c=>CategoriasBuilder.ToResponseDto(c));
    }

    public async Task<CategoriaResponseDto> GetCategoriasById(int id)
    {
        var categoria = await _categoriaRepository.GetCategoriaById(id);
        return CategoriasBuilder.ToResponseDto(categoria);
    }

    public async Task<CategoriaResponseDto> CreateCategoria(CreateCategoriaDto createCategoriaDto)
    {
        var categoria = await _categoriaRepository.CreateCategoria(CategoriasBuilder.ToEntity(createCategoriaDto));
        return CategoriasBuilder.ToResponseDto(categoria);
    }
    public async Task<CategoriaResponseDto> UpdateCategoria(int id, CreateCategoriaDto createCategoriaDto)
    {
        var categoria = await _categoriaRepository.GetCategoriaById(id);
        if(categoria == null) return null;

        var categoriaToUpdate = CategoriasBuilder.ToEntityUpdate(createCategoriaDto, id);

        return CategoriasBuilder.ToResponseDto(await _categoriaRepository.UpdateCategorias(categoriaToUpdate));
    }
    public async Task DeleteAsync(int id)
    {
        var categoria = await _categoriaRepository.GetCategoriaById(id);
        if(categoria != null)
        {
            await _categoriaRepository.DeleteCategoria(id);
        }
    }



   
}
