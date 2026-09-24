using System;

namespace EBlumbit.Dto.Ventas;

public class DetalleVentaRequest
{
    public int Cantidad { get; set; }

    public decimal PrecioUnitarioVenta { get; set; }

    public string Observaciones { get; set; }

    public int ProductoId { get; set; }

    public int AlmacenId { get; set; }


}
