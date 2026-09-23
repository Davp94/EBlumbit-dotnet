using EBlumbit.Services.spec;
using EBlumbit.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController(ICategoriaService categoriaService) : ControllerBase
    {
        private readonly ICategoriaService _categoriaService = categoriaService;

        [HttpGet]
        public async Task<ActionResult<ICollection<CategoriaResponseDto>>> GetAllCategorias()
        {
            var categorias = await _categoriaService.GetAllCategorias();
            return Ok(categorias.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaResponseDto>> GetCategoriaById(int id)
        {
            var categoria = await _categoriaService.GetCategoriasById(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaResponseDto>> CreateCategoria([FromBody] CreateCategoriaDto createCategoriaDto)
        {
            var created = await _categoriaService.CreateCategoria(createCategoriaDto);
            return Created("", created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] CreateCategoriaDto createCategoriaDto)
        {
            await _categoriaService.UpdateCategoria(id, createCategoriaDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            await _categoriaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
