using CarGalleryAPI.Data;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "HeadAdmin,Admin")]
        public async Task<IActionResult> AddBody([FromBody] Body bodyRequest)
        {
            if (_dbContext.Bodies.Where(x => x.type == bodyRequest.type).Any())
                return BadRequest($"{bodyRequest.type} is already in database");

            int currentBiggestId = await _dbContext.Bodies.MaxAsync(x => x.id);
            bodyRequest.id = currentBiggestId + 1;
            
            await _dbContext.Bodies.AddAsync(bodyRequest);
            await _dbContext.SaveChangesAsync();

            return Ok(bodyRequest);
        }

        [HttpDelete]
        [Authorize(Roles = "HeadAdmin,Admin")]
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
