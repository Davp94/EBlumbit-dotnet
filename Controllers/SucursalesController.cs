using EBlumbit.Dto.Sucursales;
using EBlumbit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers
{
    // Alias de ruta validos para ambos frontends: /api/sucursales y /sucursales.
    [Authorize]
    [Route("api/sucursales")]
    [ApiController]
    public class SucursalesController(SucursalService sucursalService) : ControllerBase
    {
        private readonly SucursalService _sucursalService = sucursalService;

        [HttpGet]
        public async Task<ActionResult<ICollection<SucursalResponseDto>>> GetAllSucursales()
        {
            var sucursales = await _sucursalService.GetAll();
            return Ok(sucursales.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SucursalResponseDto>> GetSucursalById(int id)
        {
            return Ok(await _sucursalService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<SucursalResponseDto>> CreateSucursal([FromBody] SucursalRequestDto request)
        {
            var created = await _sucursalService.Create(request);
            return Created("", created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SucursalResponseDto>> UpdateSucursal(int id, [FromBody] SucursalRequestDto request)
        {
            return Ok(await _sucursalService.Update(id, request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSucursal(int id)
        {
            await _sucursalService.Delete(id);
            return NoContent();
        }
    }
}
