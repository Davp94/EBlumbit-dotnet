using System.ComponentModel.DataAnnotations;

namespace EBlumbit.Dto.Proveedores;

public class ProveedorRequestDto
{
    [Required]
    public string RazonSocial { get; set; } = string.Empty;

    public string? NroIdentificacion { get; set; }

    [Required]
    public string Contacto { get; set; } = string.Empty;

    [Required]
    public string Telefono { get; set; } = string.Empty;

    [EmailAddress]
    public string? Correo { get; set; }

    public string? Observaciones { get; set; }
}