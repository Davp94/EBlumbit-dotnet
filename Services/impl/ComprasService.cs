using EBlumbit.Builders;
using EBlumbit.Data;
using EBlumbit.Dto.Compras;
using EBlumbit.exceptions;
using EBlumbit.Models;
using EBlumbit.Repository.spec;
using EBlumbit.Services.spec;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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

    public async Task<byte[]> GenerateCompraReportPdf(int id)
    {
        var compra = await _compraRepository.GetCompraById(id);
        var totalCompra = 0m;
        if(compra == null)
        {
            throw new ResourceNotFoundException("Compra no encontrada");
        }
        QuestPDF.Settings.License = LicenseType.Community;
        var document = Document.Create(container =>
        {
            container.Page(page => 
            {
               page.Size(PageSizes.Letter);
               page.Margin(2, Unit.Centimetre);
               page.PageColor(Colors.White);
               page.DefaultTextStyle(x=>x.FontSize(12));
               page.Header().Row(row =>
               {
                   row.RelativeItem().Column(column =>
                   {
                       column.Item().Text($"Compra - {compra.Codigo} ").SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);
                       column.Item().Text($"Fecha - {compra.Fecha:dd/MM/yyyy}");
                       column.Item().Text($"Cliente - {compra.Proveedor.RazonSocial}");
                   });
               }); 
               page.Content().PaddingVertical(2, Unit.Centimetre).Column(column =>
               {
                   column.Item().Table(table =>
                   {
                       table.ColumnsDefinition(columns =>
                       {
                           columns.RelativeColumn(3);
                           columns.RelativeColumn();
                           columns.RelativeColumn();
                           columns.RelativeColumn();
                       });
                       table.Header(header =>
                       {
                           header.Cell().Element(CellStyle).Text("Producto");
                           header.Cell().Element(CellStyle).Text("Cantidad");
                           header.Cell().Element(CellStyle).Text("Precio");
                           header.Cell().Element(CellStyle).Text("Total");
                           static IContainer CellStyle(IContainer container)
                           {
                               return container.DefaultTextStyle(x=>x.SemiBold()).Padding(3).Border(1).BorderColor(Colors.Grey.Lighten2);
                           }

                       });
                       foreach(var item in compra.DetalleCompras)
                       {
                            var totalItem = item.Cantidad * item.PrecioUnitarioCompra;
                            totalCompra += totalItem;
                            table.Cell().Element(CellStyle).Text(item.Producto.Nombre);
                            table.Cell().Element(CellStyle).Text(item.Cantidad.ToString());
                            table.Cell().Element(CellStyle).Text(item.PrecioUnitarioCompra.ToString());
                            table.Cell().Element(CellStyle).Text(totalItem.ToString());
                            static IContainer CellStyle(IContainer container)
                            {
                               return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(3);
                            }
                       }
                   });
                   column.Item().AlignRight().Text($"TOTAL: {(totalCompra - compra.DescuentoTotal)}").SemiBold().FontSize(14);
               });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Comprobante de venta!!!");
                    x.Line("https://eblumbit.site").FontSize(10).FontColor(Colors.Grey.Lighten5);
                });
            });
        });
        return document.GeneratePdf();
    }
}