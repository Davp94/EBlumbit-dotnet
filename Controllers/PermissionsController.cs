using EBlumbit.Models;
using EBlumbit.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController(PermissionService permissionService) : ControllerBase
    {
        private readonly PermissionService _permissionService = permissionService;

        [HttpGet]
        public async Task<ActionResult<ICollection<Permission>>> GetAllPermissions()
        {
            var permissions = await _permissionService.GetAllPermissions();
            return Ok(permissions.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Permission>> GetPermissionById(int id)
        {
            var permission = await _permissionService.GetPermissionById(id);
            if (permission == null) return NotFound();
            return Ok(permission);
        }

        [HttpPost]
        public async Task<ActionResult<Permission>> CreatePermission([FromBody] Permission permission)
        {
            var created = await _permissionService.CreatePermission(permission);
            return Created("", created);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePermission([FromBody] Permission permission)
        {
            await _permissionService.UpdatePermission(permission);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            await _permissionService.DeletePermission(id);
            return NoContent();
        }
    }
}
