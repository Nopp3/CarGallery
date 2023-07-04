using CarGalleryAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CarGalleryAPI.Controllers.Models;

namespace CarGalleryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : Controller
    {
        private readonly DatabaseContext _dbContext;
        public BrandsController(DatabaseContext dbContext) => _dbContext = dbContext;

        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            List<Brand> Brands = await _dbContext.Brands.ToListAsync();
            return Ok(Brands);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddBrand([FromBody] Brand brandRequest)
        {
            if (_dbContext.Brands.Where(x => x.name == brandRequest.name).Any())
                return BadRequest($"{brandRequest.name} is already in database");

            int currentBiggestId = await _dbContext.Brands.MaxAsync(x => x.id);
            brandRequest.id = currentBiggestId + 1;

            await _dbContext.Brands.AddAsync(brandRequest);
            await _dbContext.SaveChangesAsync();

            return Ok(brandRequest);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBrand([FromQuery] int id)
        {
            var brand = await _dbContext.Brands.FindAsync(id);

            if (brand == null)
                return NotFound();

            _dbContext.Brands.Remove(brand);
            await _dbContext.SaveChangesAsync();

            return Ok(brand);
        }
    }
}
