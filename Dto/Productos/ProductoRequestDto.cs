using System;

namespace EBlumbit.Dto.Productos;

public class ProductoRequestDto
{
    public string Nombre { get; set; }
    public string CodigoBarra { get; set; }
    public string UnidadMedida { get; set; }
    public string Marca { get; set; }
    public IFormFile Imagen { get; set; }
    public string Descripcion { get; set; }
    public decimal PrecioVentaActual { get; set; }
    public int StockMinimo { get; set; }
    public bool Estado { get; set; }
    public int CategoriaId {get; set;}
}
