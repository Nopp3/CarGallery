using CarGalleryAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CarGalleryAPI.Controllers.Models;
using System.Collections.Generic;

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
        public async Task<IActionResult> AddBody([FromBody] Body bodyRequest)
        {
            int currentBiggestId = await _dbContext.Bodies.MaxAsync(x => x.id);

            bodyRequest.id = currentBiggestId + 1;
            
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Bodies ON");

                await _dbContext.Bodies.AddAsync(bodyRequest);
                await _dbContext.SaveChangesAsync();

                _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Bodies OFF");

                transaction.Commit();

                return Ok(bodyRequest);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBody([FromQuery] int id)
        {
            var body = await _dbContext.Bodies.FindAsync(id);
               
            if (body == null)
                return NotFound();

            _dbContext.Bodies.Remove(body);
            await _dbContext.SaveChangesAsync();

            return Ok(body);
        }
    }
}
