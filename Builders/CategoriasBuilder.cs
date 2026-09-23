using System;
using EBlumbit.Dto;
using EBlumbit.Models;

namespace EBlumbit.Builders;

public class CategoriasBuilder
{
    public static Categoria ToEntity(CreateCategoriaDto dto)
    {
        return new Categoria
        {
            Nombre = dto.Nombre,
            Detalle = dto.Detalle
        };
    }

    public static CategoriaResponseDto ToResponseDto(Categoria categoria)
    {
        return new CategoriaResponseDto
        {
            Id=categoria.Id,
            Nombre=categoria.Nombre,
            Detalle=categoria.Detalle
        };
    }

    public static Categoria ToEntityUpdate(CreateCategoriaDto dto, int id)
    {
        return new Categoria
        {
            Id=id,
            Nombre=dto.Nombre,
            Detalle=dto.Detalle
        };
    }
}
