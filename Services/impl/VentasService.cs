using System;
using EBlumbit.Builders;
using EBlumbit.Data;
using EBlumbit.Dto.Ventas;
using EBlumbit.exceptions;
using EBlumbit.Repository.spec;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class VentasService(IVentaRepository ventaRepository, IInventarioRepository inventarioRepository, AppDbContext appDbContext, ILogger<VentasService> logger) : IVentasService
{
    private readonly IVentaRepository _ventasRepository = ventaRepository;

    private readonly IInventarioRepository _inventarioRepository = inventarioRepository; 

    private readonly AppDbContext _context = appDbContext;

    private readonly ILogger<VentasService> _logger = logger;


    public async Task<IEnumerable<VentasResponse>> FindAllVentas()
    {
        var ventas = await _ventasRepository.GetAllVentas();
        return ventas.Select(VentasBuilder.ToResponseDto);
    }

    public async Task<VentasDetailResponse?> FindVentaById(int id)
    {
        var venta = await _ventasRepository.GetVentaById(id);
        if(venta == null)
        {
            throw new ResourceNotFoundException("Venta no encontarada");
        }
        return VentasBuilder.ToDetailResponseDto(venta);
    }

    public async Task<VentasResponse> CreateVenta(CreateVentaRequest createVentaRequest)
    {
        _logger.LogInformation("Create Venta Request {createVentaRequest}", createVentaRequest);
        if(createVentaRequest == null || createVentaRequest.DetalleVenta == null || !createVentaRequest.DetalleVenta.Any())
        {
            throw new ArgumentException("La venta debe incluir al menos un producto");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            //validate stock //update inventario
            foreach(var detalle in createVentaRequest.DetalleVenta)
            {
                var inventario = await _inventarioRepository.GetByProductoAndAlmacen(detalle.ProductoId, detalle.AlmacenId);
                if(inventario == null || inventario.CantidadActual < detalle.Cantidad)
                {
                    throw new InvalidOperationException($"Stock insuficiente en inventario para el producto {detalle.ProductoId} en el almacen {detalle.AlmacenId}. Disponible: {inventario?.CantidadActual ?? 0} Solicitado: {detalle.Cantidad}");
                }
                inventario.CantidadActual -= detalle.Cantidad;
                inventario.FechaActualizacion = DateTime.UtcNow;
                await  _inventarioRepository.UpdateInventario(inventario);
            }
            // - generate data from server
            int nextSeq = await _ventasRepository.GetNextCodigoSeq();
            string codigo = $"BLUMB-{nextSeq:D5}";
            //create venta -> Venta ID
            var venta = VentasBuilder.ToEntity(createVentaRequest, codigo);
            _logger.LogInformation("Create Venta Entity {venta}", venta);
            var createdVenta = await _ventasRepository.CreateVenta(venta);

            await transaction.CommitAsync();
            return VentasBuilder.ToResponseDto(createdVenta);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task AnularVenta(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var venta  = await _ventasRepository.GetVentaById(id);
            if(venta == null)
            {
                throw new KeyNotFoundException($"Venta con Id {id} no encontrada.");
            }

            if(!venta.Estado)
            {
                throw new InvalidOperationException("La venta ya se encuentra anulada");
            }
            venta.Estado = false;
            if(venta.DetalleVentas != null)
            {
                foreach(var detalle in venta.DetalleVentas)
                {
                    var inventario = await _inventarioRepository.GetByProductoAndAlmacen(detalle.ProductoId, detalle.AlmacenId);
                    if(inventario != null)
                    {
                        inventario.CantidadActual += detalle.Cantidad;
                        inventario.FechaActualizacion = DateTime.UtcNow;
                        await _inventarioRepository.UpdateInventario(inventario);
                    }
                }
            }
            await _ventasRepository.UpdateVenta(venta);
            await transaction.CommitAsync();
        }
        catch (System.Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

}
