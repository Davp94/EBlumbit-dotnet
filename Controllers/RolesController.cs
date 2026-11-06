using EBlumbit.Dto.Roles;
using EBlumbit.exceptions;
using EBlumbit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers;

[Authorize]
[Route("api/roles")]
[ApiController]
public class RolesController(RoleService roleService) : ControllerBase
{
    private readonly RoleService _roleService = roleService;

    [HttpGet]
    public async Task<ActionResult<ICollection<RoleResponseDto>>> GetAll()
    {
        var roles = await _roleService.GetAllAsync();
        return Ok(roles.ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleResponseDto>> GetById(int id)
    {
        var role = await _roleService.GetByIdAsync(id);
        if (role == null) throw new ResourceNotFoundException($"Rol con id {id} no encontrado");
        return Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult<RoleResponseDto>> Create([FromBody] RoleRequestDto request)
    {
        var created = await _roleService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RoleResponseDto>> Update(int id, [FromBody] RoleRequestDto request)
    {
        var updated = await _roleService.UpdateAsync(id, request);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _roleService.DeleteAsync(id);
        return NoContent();
    }
}
