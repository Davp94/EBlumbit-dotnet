using System;

namespace EBlumbit.Models;

public class Venta
{
    public int Id { get; set;}
    public string Codigo { get; set; }

    public DateTime Fecha { get; set; }

    public decimal DescuentoTotal { get; set; }

    public bool Estado { get; set; }

    public string Detalle { get; set; }

    public string Observacion { get; set; }

    public int ClienteId { get; set; }

    public int UsuarioId { get; set; }

    public Users Users { get; set; }

    public Cliente cliente { get; set; }


}
