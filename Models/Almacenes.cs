using System;

namespace EBlumbit.Models;

public class Almacenes
{
    public int Id { get; set; }

    public string Codigo { get; set;}

    public string Nombre { get; set;}

    public string? Descripcion { get; set;}

    public int SucursalId { get; set;}
    public Sucursales Sucursal { get; set; }
}
