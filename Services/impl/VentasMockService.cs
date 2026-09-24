using System;
using EBlumbit.Dto.Ventas;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class VentasMockService : IVentasService
{
    public Task AnularVenta(int id)
    {
        Console.WriteLine("COMUNICAICON EXITOSA ANUAR VENTA");
        
        throw new NotImplementedException();
    }

    public Task<VentasResponse> CreateVenta(CreateVentaRequest createVentaRequest)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<VentasResponse>> FindAllVentas()
    {
        throw new NotImplementedException();
    }

    public Task<VentasDetailResponse> FindVentaById(int id)
    {
        throw new NotImplementedException();
    }
}
