using System;
using EBlumbit.Builders;
using EBlumbit.Dto;
using EBlumbit.exceptions;
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
        if (categoria == null)
        {
            throw new ResourceNotFoundException($"Categoria con id {id} no encontrado");
        }
        return CategoriasBuilder.ToResponseDto(categoria);
    }

    public async Task<CategoriaResponseDto> CreateCategoria(CreateCategoriaDto createCategoriaDto)
    {
        var categoria = await _categoriaRepository.CreateCategoria(CategoriasBuilder.ToEntity(createCategoriaDto));
        return CategoriasBuilder.ToResponseDto(categoria);
    }
    public async Task<CategoriaResponseDto> UpdateCategoria(int id, CreateCategoriaDto createCategoriaDto)
    {
        // Se actualiza la entidad ya rastreada para evitar conflictos de tracking en EF.
        var categoria = await _categoriaRepository.GetCategoriaById(id);
        if (categoria == null)
        {
            throw new ResourceNotFoundException($"Categoria con id {id} no encontrado");
        }

        categoria.Nombre = createCategoriaDto.Nombre;
        categoria.Detalle = createCategoriaDto.ResolveDetalle();

        return CategoriasBuilder.ToResponseDto(await _categoriaRepository.UpdateCategorias(categoria));
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
