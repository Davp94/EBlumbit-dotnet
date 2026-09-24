using EBlumbit.Dto.Ventas;
using EBlumbit.Services.spec;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController(IVentasService ventasService) : ControllerBase
    {
        private readonly IVentasService _ventasService = ventasService;

        [HttpGet]
        public async Task<ActionResult<ICollection<VentasResponse>>> GetAllVentas()
        {
            var ventas = await _ventasService.FindAllVentas();
            return Ok(ventas.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VentasDetailResponse>> GetVentasResponseById(int id)
        {
            var ventas = await _ventasService.FindVentaById(id);
            if (ventas == null) return NotFound();
            return Ok(ventas);
        }

        [HttpPost]
        public async Task<ActionResult<VentasResponse>> CreateVentasResponse([FromBody] CreateVentaRequest createVentaRequest)
        {
            var created = await _ventasService.CreateVenta(createVentaRequest);
            return Created("", created);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> AnularVenta(int id)
        {
            await _ventasService.AnularVenta(id);
            return NoContent();
        }
    }
}
