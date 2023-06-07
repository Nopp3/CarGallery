using CarGalleryAPI.Models;
using CarGalleryAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddBody([FromBody] string name)
        {
            Brand brand = new Brand();
            brand.name = name;

            int currentBiggestId = await _dbContext.Brands.MaxAsync(x => x.id);

            brand.id = currentBiggestId+1;

            await _dbContext.Brands.AddAsync(brand);
            await _dbContext.SaveChangesAsync();

            return Ok(brand);
        }
    }
}
