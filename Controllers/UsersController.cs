using EBlumbit.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers
{
    [Route("api/[controller]")] // /api/users
    [ApiController]
    public class UsersController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpGet]
        public async Task<ICollection<Users>> GetAllUsers()
        {
            return _userService.GetAllUsers().Result.ToList();
        }

        [HttpGet("{id}")]
        public async Task<Users> GetUserById(int id)
        {
            return _userService.GetUserById(id).Result;
        }

        [HttpPost]
        public async Task<Users> CreateUser([FromBody]Users user)
        {
            return _userService.CreateUser(user).Result;
        }

        [HttpPut]
        public async Task<Users> UpdateUser([FromBody] Users user)
        {
            return _userService.UpdateUser(user).Result;
        }

        [HttpDelete("{id}")]
        public async Task DeleteUser(int id)
        {
            await _userService.DeleteUsuario(id);
        }

        [HttpPatch("{id}")]
        public async Task LogicalDeleteUser(int id)
        {
            await _userService.LogicalDeleteUsuario(id);
        }
    }
}
