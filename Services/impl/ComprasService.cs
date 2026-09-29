using EBlumbit.Builders;
using EBlumbit.Data;
using EBlumbit.Dto.Compras;
using EBlumbit.Models;
using EBlumbit.Repository.spec;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class ComprasService(ICompraRepository compraRepository, IInventarioRepository inventarioRepository, AppDbContext appDbContext) : IComprasService
{
    private const string EstadoRecibida = "RECIBIDA";
    private const string EstadoAnulada = "ANULADA";
    private readonly ICompraRepository _compraRepository = compraRepository;
    private readonly IInventarioRepository _inventarioRepository = inventarioRepository;
    private readonly AppDbContext _context = appDbContext;

    public async Task<IEnumerable<CompraResponse>> FindAllCompras()
    {
        var compras = await _compraRepository.GetAllCompras();
        return compras.Select(ComprasBuilder.ToResponseDto);
    }

    public async Task<CompraDetailResponse?> FindCompraById(int id)
    {
        var compra = await _compraRepository.GetCompraById(id);
        return compra == null ? null : ComprasBuilder.ToDetailResponseDto(compra);
    }

    public async Task<CompraResponse> CreateCompra(CreateCompraRequest request)
    {
        if (request.DetalleCompra == null || request.DetalleCompra.Count == 0)
        {
            throw new ArgumentException("La compra debe incluir al menos un producto.");
        }

        if (request.DetalleCompra.Any(d => d.Cantidad <= 0 || d.PrecioUnitarioCompra < 0))
        {
            throw new ArgumentException("Las cantidades deben ser mayores a cero y los precios no pueden ser negativos.");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var detalle in request.DetalleCompra)
            {
                var inventario = await _inventarioRepository.GetByProductoAndAlmacen(detalle.ProductoId, detalle.AlmacenId);
                if (inventario == null)
                {
                    inventario = new Inventario
                    {
                        ProductoId = detalle.ProductoId,
                        AlmacenId = detalle.AlmacenId,
                        CantidadActual = detalle.Cantidad,
                        FechaActualizacion = DateTime.UtcNow
                    };
                    await _inventarioRepository.CreateInventario(inventario);
                }
                else
                {
                    inventario.CantidadActual += detalle.Cantidad;
                    inventario.FechaActualizacion = DateTime.UtcNow;
                    await _inventarioRepository.UpdateInventario(inventario);
                }
            }

            var codigo = $"BLUMC-{await _compraRepository.GetNextCodigoSeq():D5}";
            var compra = ComprasBuilder.ToEntity(request, codigo);
            var created = await _compraRepository.CreateCompra(compra);
            await transaction.CommitAsync();
            return ComprasBuilder.ToResponseDto(created);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task AnularCompra(int id)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var compra = await _compraRepository.GetCompraById(id);
            if (compra == null)
            {
                throw new KeyNotFoundException($"Compra con Id {id} no encontrada.");
            }

            if (compra.Estado != EstadoRecibida)
            {
                throw new InvalidOperationException("Solo se pueden anular compras recibidas.");
            }

            foreach (var detalle in compra.DetalleCompras)
            {
                var inventario = await _inventarioRepository.GetByProductoAndAlmacen(detalle.ProductoId, detalle.AlmacenId);
                if (inventario == null || inventario.CantidadActual < detalle.Cantidad)
                {
                    throw new InvalidOperationException($"No se puede anular la compra: stock insuficiente para el producto {detalle.ProductoId} en el almacen {detalle.AlmacenId}.");
                }

                inventario.CantidadActual -= detalle.Cantidad;
                inventario.FechaActualizacion = DateTime.UtcNow;
                await _inventarioRepository.UpdateInventario(inventario);
            }

            compra.Estado = EstadoAnulada;
            await _compraRepository.UpdateCompra(compra);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}