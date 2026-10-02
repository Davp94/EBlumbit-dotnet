namespace EBlumbit.Dto.Auth;

public record class AuthResponse
{
    public string AccessToken;

    public string RefreshToken;

    public int UsuarioId;

    public int ExpirationMinutes;

    //----------------------------

    public string Email;
    public List<string> Roles;

    public List<string> Permissions;
}
