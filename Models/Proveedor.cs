namespace EBlumbit.Models;

public class Proveedor
{
    public int Id { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string? NroIdentificacion { get; set; }
    public string Contacto { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Correo { get; set; }
    public string? Observaciones { get; set; }
    public bool? Estado { get; set; }
    public ICollection<Compra> Compras { get; set; } = [];
}