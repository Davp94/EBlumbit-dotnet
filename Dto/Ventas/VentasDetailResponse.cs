using System;

namespace EBlumbit.Dto.Ventas;

public class VentasDetailResponse : VentasResponse
{
    public List<DetalleVentaResponse> DetalleVenta;
}
