using System;

namespace EBlumbit.Dto.Productos;

public class ProductoResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string CodigoBarra { get; set; }
    public string UnidadMedida { get; set; }
    public string Marca { get; set; }
    public string Imagen { get; set; }
    public string Descripcion { get; set; }
    public decimal PrecioVentaActual { get; set; }
    public int StockMinimo { get; set; }
    public bool Estado { get; set; }
    public string FechaRegistro { get; set; }
    public int CategoriaId {get; set;}
    public string CategoriaNombre {get; set;}
}
