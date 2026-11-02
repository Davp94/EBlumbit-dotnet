using EBlumbit.Dto.Permissions;
using EBlumbit.exceptions;
using EBlumbit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers
{
    // Alias de ruta validos para ambos frontends.
    [Authorize]
    [Route("api/permissions")]
    [ApiController]
    public class PermissionsController(PermissionService permissionService) : ControllerBase
    {
        private readonly PermissionService _permissionService = permissionService;

        [HttpGet]
        public async Task<ActionResult<ICollection<PermissionResponseDto>>> GetAllPermissions()
        {
            var permissions = await _permissionService.GetAllPermissions();
            return Ok(permissions.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionResponseDto>> GetPermissionById(int id)
        {
            var permission = await _permissionService.GetPermissionById(id);
            if (permission == null) throw new ResourceNotFoundException($"Permiso con id {id} no encontrado");
            return Ok(permission);
        }

        [HttpPost]
        public async Task<ActionResult<PermissionResponseDto>> CreatePermission([FromBody] PermissionRequestDto request)
        {
            var created = await _permissionService.CreatePermission(request);
            return Created("", created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PermissionResponseDto>> UpdatePermission(int id, [FromBody] PermissionRequestDto request)
        {
            return Ok(await _permissionService.UpdatePermission(id, request));
        }

        // Alias legado: el id viaja en el body.
        [HttpPut]
        public async Task<ActionResult<PermissionResponseDto>> UpdatePermissionFromBody([FromBody] PermissionRequestDto request)
        {
            if (request.Id == null) throw new BusinessRuleException("El id del permiso es obligatorio");
            return Ok(await _permissionService.UpdatePermission(request.Id.Value, request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            await _permissionService.DeletePermission(id);
            return NoContent();
        }
    }
}
