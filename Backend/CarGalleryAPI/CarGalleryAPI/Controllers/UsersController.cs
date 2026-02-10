using CarGalleryAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CarGalleryAPI.Controllers.Models;
using System.Security.Claims;

namespace CarGalleryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public UsersController(DatabaseContext dbContext) => _dbContext = dbContext;

        private bool TryGetAuthenticatedUserId(out Guid userId)
        {
            userId = Guid.Empty;
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdValue, out userId);
        }

        private bool IsAdmin()
        {
            return User.IsInRole(Roles.HeadAdmin.ToString()) || User.IsInRole(Roles.Admin.ToString());
        }

        [HttpGet("all")]
        [Authorize(Roles = "HeadAdmin,Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _dbContext.Users
                .Select(u => new
                {
                    u.id,
                    u.role_id,
                    u.username,
                    u.email
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser([FromQuery(Name = "id")] Guid id)
        {
            if (!TryGetAuthenticatedUserId(out var currentUserId))
                return Unauthorized();

            if (!IsAdmin() && id != currentUserId)
                return Forbid();

            var user = await _dbContext.Users
                .Where(x => x.id == id)
                .Select(u => new
                {
                    u.id,
                    u.role_id,
                    u.username,
                    u.email
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();
            else return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User userRequest)
        {
            if (userRequest == null)
                return BadRequest();
            else
            {
                if (_dbContext.Users.Where(x => x.username == userRequest.username).Any())
                    return BadRequest($"User {userRequest.username} is already in database");

                userRequest.id = Guid.NewGuid();
                userRequest.role_id = (int)Roles.User;
                userRequest.password = Hash.HashPassword(userRequest.password);

                if (!userRequest.email.Contains('@'))
                    userRequest.email = null;
                await _dbContext.Users.AddAsync(userRequest);
                await _dbContext.SaveChangesAsync();

                return Ok(userRequest);
            }
        }
    }
}
