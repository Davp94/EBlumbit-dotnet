namespace EBlumbit.Models;

public class Compra
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int ProveedorId { get; set; }
    public int UsuarioId { get; set; }
    public decimal? DescuentoTotal { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Detalle { get; set; }
    public string? Observaciones { get; set; }
    public Proveedor Proveedor { get; set; } = null!;
    public Users Usuario { get; set; } = null!;
    public ICollection<DetalleCompra> DetalleCompras { get; set; } = [];
}