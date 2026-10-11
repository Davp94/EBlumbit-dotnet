using EBlumbit.Dto.Clientes;
using EBlumbit.Services.spec;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientesController(IClienteService clienteService) : ControllerBase
{
    private readonly IClienteService _clienteService = clienteService;

    [HttpGet]
    public async Task<ActionResult<ICollection<ClienteResponseDto>>> GetAllClientes()
    {
        var clientes = await _clienteService.GetAllClientes();
        return Ok(clientes.ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteResponseDto>> GetClienteById(int id)
    {
        var cliente = await _clienteService.GetClienteById(id);
        if (cliente == null) return NotFound();
        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteResponseDto>> CreateCliente([FromBody] ClienteRequestDto requestDto)
    {
        var created = await _clienteService.CreateCliente(requestDto);
        return CreatedAtAction(nameof(GetClienteById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClienteResponseDto>> UpdateCliente(int id, [FromBody] ClienteRequestDto requestDto)
    {
        var updated = await _clienteService.UpdateCliente(id, requestDto);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCliente(int id)
    {
        await _clienteService.DeleteCliente(id);
        return NoContent();
    }
}