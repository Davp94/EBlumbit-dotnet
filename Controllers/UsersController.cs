using EBlumbit.Dto.Users;
using EBlumbit.exceptions;
using EBlumbit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers
{
    // Rutas validas para ambos frontends: /usuarios (Angular) y /api/usuarios (React).
    [Authorize]
    [Route("api/usuarios")]
    [Route("usuarios")]
    [Route("api/users")]
    [Route("users")]
    [ApiController]
    public class UsersController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpGet]
        [Authorize(Policy = "leer:users")]
        public async Task<ActionResult<ICollection<UserResponseDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users.ToList());
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "leer:users")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) throw new ResourceNotFoundException($"Usuario con id {id} no encontrado");
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Policy = "crear:users")]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] UserRequestDto request)
        {
            var created = await _userService.CreateUser(request);
            return Created("", created);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "actualizar:users")]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(int id, [FromBody] UserRequestDto request)
        {
            var updated = await _userService.UpdateUser(id, request);
            return Ok(updated);
        }

        // Alias legado: el id viaja en el body.
        [HttpPut]
        [Authorize(Policy = "actualizar:users")]
        public async Task<ActionResult<UserResponseDto>> UpdateUserFromBody([FromBody] UserRequestDto request)
        {
            if (request.Id == null) throw new BusinessRuleException("El id del usuario es obligatorio");
            var updated = await _userService.UpdateUser(request.Id.Value, request);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "eliminar:users")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUsuario(id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "eliminar:users")]
        public async Task<IActionResult> LogicalDeleteUser(int id)
        {
            await _userService.LogicalDeleteUsuario(id);
            return NoContent();
        }
    }
}
