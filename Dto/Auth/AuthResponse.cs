namespace EBlumbit.Dto.Auth;

public record AuthResponse
(
    string AccessToken,
    string RefreshToken,
    int UsuarioId,
    int ExpirationMinutes,
    string Email,
    List<string> Roles,
    List<string> Permissions
);
