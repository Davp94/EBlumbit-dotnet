using EBlumbit.Dto.Common;
using EBlumbit.Dto.Productos;
using EBlumbit.Services.spec;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController(IProductoService productoService) : ControllerBase
    {
        private readonly IProductoService _productoService = productoService;

        [HttpGet]
        public async Task<ActionResult<PageResult<ProductoResponseDto>>> GetProductosPaginacion([FromQuery] ProductoQueryParams queryParams)
        {
            var result = await _productoService.GetProductosPagination(queryParams);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoResponseDto>> GetProductoById(int id)
        {
            var producto = await _productoService.GetProductoById(id);
            return Ok(producto);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductoResponseDto>> CreateProducto([FromForm] ProductoRequestDto requestDto)
        {
            var created = await _productoService.CreateProducto(requestDto);
            return CreatedAtAction(nameof(GetProductoById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductoResponseDto>> UpdateProducto(int id, [FromBody] ProductoRequestDto requestDto)
        {
            var updated = await _productoService.UpdateProducto(id, requestDto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            await _productoService.DeleteProducto(id);
            return NoContent();
        }
    }
}
