using EBlumbit.Dto.Compras;

namespace EBlumbit.Services.spec;

public interface IComprasService
{
    Task<IEnumerable<CompraResponse>> FindAllCompras();
    Task<CompraDetailResponse?> FindCompraById(int id);
    Task<CompraResponse> CreateCompra(CreateCompraRequest request);
    Task AnularCompra(int id);

    Task<byte[]> GenerateCompraReportPdf(int id);
}