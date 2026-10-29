namespace EBlumbit.Dto.Users;

/// <summary>
/// Contrato de usuario compartido por ambos backends.
/// Incluye alias (correo) para mantener compatibles a los dos frontends.
/// </summary>
public class UserResponseDto
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Correo => Email;

    public bool State { get; set; }

    public List<int> RolIds { get; set; } = [];

    public List<int> SucursalId { get; set; } = [];

    public List<int> PermissionsIds { get; set; } = [];
}
