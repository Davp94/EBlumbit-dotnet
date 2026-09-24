using System;
using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto;

public class UsuarioDto
{
    [Required(ErrorMessage = "El username es obligatorio")]
    [StringLength(50)]
    public string Username;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [StringLength(200)]
    [EmailAddress] // dsadsad@dsadsa.es
    public string Correo;

    [Required(ErrorMessage = "El password es obligatorio")]
    [StringLength(16, MinimumLength = 8)]
    [RegularExpression(@"^(?=.{8,16}$)(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).*$", ErrorMessage = "La contraseña debe tener entre 8 y 16 caracteres, al menos una mayúscula, un número y un carácter especial")]
    public string Password;
}
