using AuthUserApi.Models;
using AuthUserApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var users = await _context.Users.ToListAsync();
            if(users is not null)
            {
                return StatusCode(201, new { message = "Felhasználók: ", result = users });
            }

            return StatusCode(400, new { message = "Hiba", result = users });   
        }

        [HttpGet("getuserbyid/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is not null)
            {
                return StatusCode(201, new { message = "Felhasználó: ", result = user });
            }

            return StatusCode(400, new { message = "Hiba", result = user });
        }

        [HttpPut("updateuser")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateDto updateDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
            {
                return StatusCode(404, new { message = "Felhasználó nem található" });
            }

            user.Name = updateDto.Name;
            user.Email = updateDto.Email;
            user.Password = updateDto.Password;

            await _context.SaveChangesAsync();
            return StatusCode(200, new { message = "Felhasználó frissítve", result = user });
        }

        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
            {
                return StatusCode(404, new { message = "Felhasználó nem található" });
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return StatusCode(200, new { message = "Felhasználó törölve", result = user });
        }
    }
}
