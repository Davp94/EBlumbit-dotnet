using System;

namespace EBlumbit.Models;

public class DetalleVenta
{
    public int Id { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecionUnitarioVenta { get; set; }

    public string Observaciones { get; set; }

    public int ProductoId { get; set; }

    public int VentaId { get; set; }

    public int AlmacenId { get; set; }

    public Productos Producto { get; set; }

    public Venta Venta { get; set; }

    public Almacenes Almacen { get; set; }


}
