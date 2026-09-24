using System;
using EBlumbit.Dto.Ventas;

namespace EBlumbit.Services.spec;

public interface IVentasService
{
    Task<IEnumerable<VentasResponse>> FindAllVentas();

    Task<VentasDetailResponse> FindVentaById(int id);

    Task<VentasResponse> CreateVenta(CreateVentaRequest createVentaRequest);

    Task AnularVenta(int id);
}
