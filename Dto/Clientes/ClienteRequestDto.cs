using System;
using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto.Clientes;

public class ClienteRequestDto
{
    [Required]
    public string NombreCompleto { get; set; }
    [Required]
    public string NroIdentificacion { get; set; }
    [Required]
    [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$")]
    public string FechaNacimiento { get; set; }
    [Required]
    public string Telefono { get; set; }
    
    [Required]
    [EmailAddress]
    public string Correo { get; set; }
}
