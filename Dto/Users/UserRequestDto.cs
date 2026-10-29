namespace EBlumbit.Dto.Users;

/// <summary>
/// Superset compatible con ambos frontends (Angular y React):
/// acepta "email" o su alias "correo", y "username" o su alias "name".
/// Si no llega password, se asigna una por defecto (los formularios actuales no la envían).
/// </summary>
public class UserRequestDto
{
    public int? Id { get; set; }

    public string? Username { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Correo { get; set; }

    public string? Password { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public List<int>? RolIds { get; set; }

    public bool? State { get; set; }
}
