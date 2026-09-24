using System;

namespace EBlumbit.Dto.Ventas;

public class DetalleVentaResponse
{
    public int Id { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitarioVenta { get; set; }

    public string Observaciones { get; set; }

    public int ProductoId { get; set; }

    public string ProductoNombre { get; set; }

    public int VentaId { get; set; }

    public int AlmacenId { get; set; }

    public string AlmacenNombre { get; set; }

}
