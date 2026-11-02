namespace EBlumbit.Dto;

public class CategoriaResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string? Detalle { get; set; }

    /// <summary>Alias de Spring para "detalle".</summary>
    public string? Descripcion => Detalle;
}
