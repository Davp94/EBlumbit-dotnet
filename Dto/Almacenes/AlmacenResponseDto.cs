namespace EBlumbit.Dto.Almacenes;

public class AlmacenResponseDto
{
    public int Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public int SucursalId { get; set; }

    public string SucursalNombre { get; set; } = string.Empty;
}
