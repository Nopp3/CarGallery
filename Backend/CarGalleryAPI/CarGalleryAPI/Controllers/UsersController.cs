using CarGalleryAPI.Models;
using CarGalleryAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public UsersController(DatabaseContext dbContext) => _dbContext = dbContext;

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.id == id);

            if (user == null)
                return NotFound();
            else return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] string username, string password)
        {
            if (_dbContext.Users.Where(x => x.username == username).Any())
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.username == username);

                if (user.password == Hash.Encrypt(password))
                    return Ok(user);
                else return BadRequest("Invalid password");
            }
            return NotFound($"Can't find user: {username}");
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();
            else
            {
                if (_dbContext.Users.Where(x => x.username == user.username).Any())
                    return BadRequest(user.username);

                user.id = Guid.NewGuid();
                user.role_id = (int)Roles.User;
                user.password = Hash.Encrypt(user.password);
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return Ok(user);
            }
        }
    }
}
