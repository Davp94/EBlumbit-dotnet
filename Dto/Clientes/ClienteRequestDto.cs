using System;
using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto.Clientes;

public class ClienteRequestDto
{
    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    public string NombreCompleto { get; set; }

    [Required(ErrorMessage = "El numero de identificacion es obligatorio")]
    public string NroIdentificacion { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$",
        ErrorMessage = "La fecha de nacimiento debe tener formato yyyy-MM-dd")]
    public string FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El telefono es obligatorio")]
    public string Telefono { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato valido")]
    public string Correo { get; set; }
}
