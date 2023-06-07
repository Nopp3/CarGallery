using CarGalleryAPI.Models;
using CarGalleryAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BodiesController : Controller
    {
        private readonly DatabaseContext _dbContext;
        public BodiesController(DatabaseContext dbContext) => _dbContext = dbContext;

        [HttpGet]
        public async Task<IActionResult> GetBodies()
        {
            List<Body> Bodies = await _dbContext.Bodies.ToListAsync();
            return Ok(Bodies);
        }

        [HttpPost]
        public async Task<IActionResult> AddBody([FromBody] string type)
        {
            Body body = new Body();
            body.type = type;

            int currentBiggestId = await _dbContext.Bodies.MaxAsync(x => x.id);

            body.id = currentBiggestId + 1;

            await _dbContext.Bodies.AddAsync(body);
            await _dbContext.SaveChangesAsync();

            return Ok(body);
        }
    }
}
