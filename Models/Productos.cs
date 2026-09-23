using System;

namespace EBlumbit.Models;

public class Productos
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string Marca { get; set; }

    public string Imagen { get; set; }

    public string Descripcion { get; set; }

    public decimal PrecioVentaActual { get; set; }

    public int CategoriaId {get; set;}

    public Categoria Categoria {get; set;}
}
