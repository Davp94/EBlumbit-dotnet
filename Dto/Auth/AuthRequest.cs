using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto.Auth;

public record AuthRequest
(
    [Required]  
    [EmailAddress]
    string Email,  
    [Required]
    string Password  
);
