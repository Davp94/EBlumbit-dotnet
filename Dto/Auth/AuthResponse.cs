namespace EBlumbit.Dto.Auth;

/// <summary>
/// Contrato de auth compartido por ambos backends.
/// Incluye alias (identifier, expiration) que consumen los frontends Angular y React.
/// </summary>
public record AuthResponse
(
    string AccessToken,
    string RefreshToken,
    int UsuarioId,
    int ExpirationMinutes,
    string Email,
    List<string> Roles,
    List<string> Permissions,
    int Identifier,
    long Expiration
);
