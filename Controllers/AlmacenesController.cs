using EBlumbit.Dto.Almacenes;
using EBlumbit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers
{
    [Authorize]
    [Route("api/almacenes")]
    [ApiController]
    public class AlmacenesController(AlmacenService almacenService) : ControllerBase
    {
        private readonly AlmacenService _almacenService = almacenService;

        [HttpGet]
        public async Task<ActionResult<ICollection<AlmacenResponseDto>>> GetAllAlmacenes()
        {
            var almacenes = await _almacenService.GetAll();
            return Ok(almacenes.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlmacenResponseDto>> GetAlmacenById(int id)
        {
            return Ok(await _almacenService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<AlmacenResponseDto>> CreateAlmacen([FromBody] AlmacenRequestDto request)
        {
            var created = await _almacenService.Create(request);
            return Created("", created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AlmacenResponseDto>> UpdateAlmacen(int id, [FromBody] AlmacenRequestDto request)
        {
            return Ok(await _almacenService.Update(id, request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlmacen(int id)
        {
            await _almacenService.Delete(id);
            return NoContent();
        }
    }
}
