namespace EBlumbit.Dto.Permissions;

/// <summary>
/// Contrato de permisos compartido por ambos backends.
/// "detalle" es el nombre canonico en .NET; "descripcion" es el alias de Spring.
/// </summary>
public class PermissionResponseDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Detalle { get; set; }

    public string? Descripcion => Detalle;

    public string Subject { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;
}
