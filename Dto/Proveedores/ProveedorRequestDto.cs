using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto.Proveedores;

public class ProveedorRequestDto
{
    [Required(ErrorMessage = "La razon social es obligatoria")]
    public string RazonSocial { get; set; } = string.Empty;

    public string? NroIdentificacion { get; set; }

    [Required(ErrorMessage = "El contacto es obligatorio")]
    public string Contacto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El telefono es obligatorio")]
    public string Telefono { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "El correo no tiene un formato valido")]
    public string? Correo { get; set; }

    public string? Observaciones { get; set; }
}
