namespace EBlumbit.Dto.Compras;

public class CompraResponse
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int ProveedorId { get; set; }
    public string ProveedorRazonSocial { get; set; } = string.Empty;
    public string? ProveedorNroIdentificacion { get; set; }
    public int UsuarioId { get; set; }
    public string Username { get; set; } = string.Empty;
    public decimal? DescuentoTotal { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Detalle { get; set; }
    public string? Observaciones { get; set; }
}