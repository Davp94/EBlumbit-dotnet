using System;
using EBlumbit.Dto;

namespace EBlumbit.Services.spec;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaResponseDto>> GetAllCategorias();
    Task<CategoriaResponseDto> GetCategoriasById(int id);
    Task<CategoriaResponseDto> CreateCategoria(CreateCategoriaDto createCategoriaDto);
    Task<CategoriaResponseDto> UpdateCategoria(int id, CreateCategoriaDto createCategoriaDto);
    Task DeleteAsync(int id);
}
