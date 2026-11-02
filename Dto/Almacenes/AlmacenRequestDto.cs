using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto.Almacenes;

public class AlmacenRequestDto
{
    [Required(ErrorMessage = "El codigo es obligatorio")]
    public string Codigo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "La sucursal es obligatoria")]
    public int SucursalId { get; set; }
}
