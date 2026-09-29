namespace EBlumbit.Models;

public class DetalleCompra
{
    public int Id { get; set; }
    public int CompraId { get; set; }
    public int ProductoId { get; set; }
    public int AlmacenId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitarioCompra { get; set; }
    public string? Observaciones { get; set; }
    public Compra Compra { get; set; } = null!;
    public Productos Producto { get; set; } = null!;
    public Almacenes Almacen { get; set; } = null!;
}