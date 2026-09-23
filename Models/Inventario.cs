using System;

namespace EBlumbit.Models;

public class Inventario
{

    public int ProductoId {get; set;}

    public int AlmacenId {get; set;}

    public Productos producto {get; set; }

    public Almacenes almacen {get; set;}

    public int CantidadActual { get; set;}

    public DateTime  FechaActualizacion {get; set;}
}
