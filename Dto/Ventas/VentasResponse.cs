using System;

namespace EBlumbit.Dto.Ventas;

public class VentasResponse
{
    public int Id { get; set; }

    public string Codigo { get; set; }

    public string Fecha { get; set; }

    public decimal DescuentoTotal { get; set; }

    public string Observacion { get; set; }

    public int UsuarioId { get; set; }

    public string Username { get; set; }

    public int ClienteId { get; set; }

    public string NroIdentificacionCliente { get; set; }


}
