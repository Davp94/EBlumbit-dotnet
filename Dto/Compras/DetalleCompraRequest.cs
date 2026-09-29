namespace EBlumbit.Dto.Compras;

public class DetalleCompraRequest
{
    public int ProductoId { get; set; }
    public int AlmacenId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitarioCompra { get; set; }
    public string? Observaciones { get; set; }
}