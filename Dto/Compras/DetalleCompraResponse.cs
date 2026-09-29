namespace EBlumbit.Dto.Compras;

public class DetalleCompraResponse
{
    public int Id { get; set; }
    public int CompraId { get; set; }
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public int AlmacenId { get; set; }
    public string AlmacenNombre { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitarioCompra { get; set; }
    public string? Observaciones { get; set; }
}