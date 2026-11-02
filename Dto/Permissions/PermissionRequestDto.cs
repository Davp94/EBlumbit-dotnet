using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto.Permissions;

/// <summary>
/// Acepta "detalle" (.NET) o su alias "descripcion" (Spring).
/// </summary>
public class PermissionRequestDto
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public string? Detalle { get; set; }

    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El subject es obligatorio")]
    [MaxLength(100)]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "La action es obligatoria")]
    [MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    public string? ResolveDetalle() => Detalle ?? Descripcion;
}
