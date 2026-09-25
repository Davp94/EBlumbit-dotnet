using System;
using EBlumbit.Dto.Ventas;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class VentasService : IVentasService
{
    public async Task AnularVenta(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<VentasResponse> CreateVenta(CreateVentaRequest createVentaRequest)
    {
        //validate stock
        //create venta -> Venta ID
        // - generate data from server
        //create detalleVenta
        //update inventario
        //return data
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<VentasResponse>> FindAllVentas()
    {
        throw new NotImplementedException();
    }

    public async Task<VentasDetailResponse> FindVentaById(int id)
    {
        throw new NotImplementedException();
    }
}
