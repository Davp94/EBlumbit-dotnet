using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto.Auth;

public record class RefreshTokenRequest
{
    [Required]
    public string RefreshToken;
}
