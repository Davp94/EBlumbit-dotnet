using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto.Roles;

public class RoleRequestDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public List<int> PermisosIds { get; set; } = [];
}
