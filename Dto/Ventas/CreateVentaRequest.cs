using System;

namespace EBlumbit.Dto.Ventas;

public class CreateVentaRequest
{
    public decimal DescuentoTotal { get; set;}

    public string Observacion {get; set; }

    public int ClienteId { get; set; }

    public int UsuarioId { get; set; } 

    public List<DetalleVentaRequest> DetalleVenta { get; set; }
}
