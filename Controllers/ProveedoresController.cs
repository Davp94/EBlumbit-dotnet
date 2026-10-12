using EBlumbit.Dto.Proveedores;
using EBlumbit.Services.spec;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProveedoresController(IProveedorService proveedorService) : ControllerBase
{
    private readonly IProveedorService _proveedorService = proveedorService;

    [HttpGet]
    public async Task<ActionResult<ICollection<ProveedorResponseDto>>> GetAllProveedores()
    {
        var proveedores = await _proveedorService.GetAllProveedores();
        return Ok(proveedores.ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProveedorResponseDto>> GetProveedorById(int id)
    {
        var proveedor = await _proveedorService.GetProveedorById(id);
        if (proveedor == null) return NotFound();
        return Ok(proveedor);
    }

    [HttpPost]
    public async Task<ActionResult<ProveedorResponseDto>> CreateProveedor([FromBody] ProveedorRequestDto requestDto)
    {
        var created = await _proveedorService.CreateProveedor(requestDto);
        return CreatedAtAction(nameof(GetProveedorById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProveedorResponseDto>> UpdateProveedor(int id, [FromBody] ProveedorRequestDto requestDto)
    {
        var updated = await _proveedorService.UpdateProveedor(id, requestDto);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProveedor(int id)
    {
        await _proveedorService.DeleteProveedor(id);
        return NoContent();
    }
}