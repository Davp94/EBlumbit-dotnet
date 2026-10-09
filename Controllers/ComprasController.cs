using EBlumbit.Dto.Compras;
using EBlumbit.Services.spec;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComprasController(IComprasService comprasService) : ControllerBase
{
    private readonly IComprasService _comprasService = comprasService;

    [HttpGet]
    public async Task<ActionResult<ICollection<CompraResponse>>> GetAllCompras()
    {
        var compras = await _comprasService.FindAllCompras();
        return Ok(compras.ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompraDetailResponse>> GetCompraById(int id)
    {
        var compra = await _comprasService.FindCompraById(id);
        return compra == null ? NotFound() : Ok(compra);
    }

    [HttpPost]
    public async Task<ActionResult<CompraResponse>> CreateCompra([FromBody] CreateCompraRequest request)
    {
        var created = await _comprasService.CreateCompra(request);
        return CreatedAtAction(nameof(GetCompraById), new { id = created.Id }, created);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> AnularCompra(int id)
    {
        await _comprasService.AnularCompra(id);
        return NoContent();
    }

    [HttpGet("{id}/report")]
    public async Task<ActionResult> GenerateComprasReport(int id)
    {
        var reportCompra = await _comprasService.GenerateCompraReportPdf(id);
        return File(reportCompra, "application/pdf", $"compra_{id}_report.pdf");
    }
}