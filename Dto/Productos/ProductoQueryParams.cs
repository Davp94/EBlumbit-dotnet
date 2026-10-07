using System;

namespace EBlumbit.Dto.Productos;

public class ProductoQueryParams
{

    //pagination
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    //filtering
    public string Search { get; set; }
    public int CategoriaId { get; set; }
    public bool Estado { get; set; }
    public string Marca { get; set; }

    //sorting
    public string SortBy { get; set; } = "Id";
    public bool IsAscending { get; set; } = true;
}
