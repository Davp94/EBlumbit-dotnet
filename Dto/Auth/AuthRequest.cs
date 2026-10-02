using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto.Auth;

public record class AuthRequest
{
    [Required]  
    [EmailAddress]
    public string Email;  

    [Required]
    public string Password;
}
