using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto.Auth;

public record RefreshTokenRequest
(
    [Required]
    string RefreshToken
);
