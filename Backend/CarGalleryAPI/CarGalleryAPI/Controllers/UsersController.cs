using CarGalleryAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CarGalleryAPI.Controllers.Models;

namespace CarGalleryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public UsersController(DatabaseContext dbContext) => _dbContext = dbContext;

        [HttpGet("all")]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await _dbContext.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery(Name = "id")] Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.id == id);

            if (user == null)
                return NotFound();
            else return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] Login loginRequest)
        {
            if (_dbContext.Users.Where(x => x.username == loginRequest.username).Any())
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.username == loginRequest.username);

                if (Hash.VerifyPassword(loginRequest.password, user.password))
                    return Ok(user.id);

                return BadRequest("Invalid password");
            }
            return NotFound($"Can't find user: {loginRequest.username}");
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
