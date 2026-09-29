namespace EBlumbit.Dto.Compras;

public class CreateCompraRequest
{
    public int ProveedorId { get; set; }
    public int UsuarioId { get; set; }
    public decimal? DescuentoTotal { get; set; }
    public string? Detalle { get; set; }
    public string? Observaciones { get; set; }
    public List<DetalleCompraRequest> DetalleCompra { get; set; } = [];
}