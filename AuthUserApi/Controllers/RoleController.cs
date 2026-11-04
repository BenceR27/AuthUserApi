using AuthUserApi.Models;
using AuthUserApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthUserApi.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public RoleController(AuthuserContext context)
        {
            _context = context;
        }

        [HttpPost("newrole")]
        public async Task<IActionResult> NewRole([FromBody] NewRoleDto newRoleDto)
        {
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = newRoleDto.Name,
            };
            if (role is not null)
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
                return StatusCode(201, new { message = "Szerepkör sikeresen létrehozva", result = role });
            }
            return StatusCode(400, new { message = "Hiba", result = role });
        }
    }
}
