using System;
using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto;

public class CreateCategoriaDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; }

    public string? Detalle { get; set; }

    /// <summary>Alias de Spring para "detalle".</summary>
    public string? Descripcion { get; set; }

    public string? ResolveDetalle() => Detalle ?? Descripcion;
}
