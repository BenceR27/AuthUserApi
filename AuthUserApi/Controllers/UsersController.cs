using AuthUserApi.Models;
using AuthUserApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthUserApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public UsersController(AuthuserContext context)
        {
            _context = context;
        }

        [HttpPost("newuser")]
        public async Task<IActionResult> NewUser([FromBody] NewUserDto newUserDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name =newUserDto.Name,
                Email = newUserDto.Email,
                Password = newUserDto.Password,
            };
            if (user is not null)
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return StatusCode(201, new { message = "Felhasználó sikeresen létrehozva", result = user });
            }
            return StatusCode(400, new {message = "Hiba", result = user });
        }

        [HttpGet("getusers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _context.Users.ToList();
            return StatusCode(201, new { message = "Felhasználók lekérése sikeres", result = users });
        }
    }
}
