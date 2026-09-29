namespace EBlumbit.Dto.Compras;

public class CompraDetailResponse : CompraResponse
{
    public List<DetalleCompraResponse> DetalleCompra { get; set; } = [];
}