using CarGalleryAPI.Models;
using CarGalleryAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        private readonly DatabaseContext _dbContext;
        public CarsController(DatabaseContext dbContext) => _dbContext = dbContext;

        [HttpGet("all")]
        public async Task<IActionResult> GetCars()
        {
            List<Car> cars = await _dbContext.Cars.ToListAsync();
            return Ok(cars);
        }

        [HttpGet]
        //[Route("{id:Guid}")]
        public async Task<IActionResult> GetUserCars([FromQuery] Guid id)
        {
            List<Car> cars = await _dbContext.Cars.ToListAsync();
            if (cars.Where(x => x.user_id == id).Any())
            {
                return Ok(cars.Where(x => x.user_id == id));
            }
            return NotFound();
        }

        [HttpGet]
        [Route("body")]
        public async Task<IActionResult> GetCarsByBody([FromQuery] string body)
        {
            Console.WriteLine(body);

            Body? bodies = await _dbContext.Bodies.FirstOrDefaultAsync(x => x.type == body);
            List<Car> cars = await _dbContext.Cars.ToListAsync();
            if (bodies != null && cars.Where(x => x.body_id == bodies.id).Any())
            {
                return Ok(cars.Where(x => x.body_id == bodies.id));
            }
            return NotFound();
        }

        [HttpGet]
        [Route("brand")]
        public async Task<IActionResult> GetCarsByBrand([FromQuery] string brand)
        {
            Brand? brands = await _dbContext.Brands.FirstOrDefaultAsync(x => x.name == brand);
            List<Car> cars = await _dbContext.Cars.ToListAsync();
            if (brands != null && cars.Where(x => x.brand_id == brands.id).Any())
            {
                return Ok(cars.Where(x => x.brand_id == brands.id));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddCar([FromBody] Car carRequest)
        {
            carRequest.id = Guid.NewGuid();
            await _dbContext.AddAsync(carRequest);
            await _dbContext.SaveChangesAsync();

            return Ok(carRequest);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCar([FromRoute] Guid id, Car updateCarRequest)
        {
            var car = await _dbContext.Cars.FindAsync(id);

            if (car == null)
                return NotFound();

            car.model = updateCarRequest.model;
            car.fuel_id = updateCarRequest.fuel_id;
            car.imagePath = updateCarRequest.imagePath;
            car.body_id = updateCarRequest.body_id;
            car.brand_id = updateCarRequest.brand_id;
            car.engine = updateCarRequest.engine;
            car.productionYear = updateCarRequest.productionYear;
            car.horsePower = updateCarRequest.horsePower;

            await _dbContext.SaveChangesAsync();
            return Ok(car);
        }

        [HttpDelete]
        //[Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCar([FromQuery] Guid id)
        {
            var car = await _dbContext.Cars.FindAsync(id);

            if (car == null)
                return NotFound();

            _dbContext.Cars.Remove(car);
            await _dbContext.SaveChangesAsync();

            return Ok(car);
        }


        [HttpGet("Fuels")]
        public async Task<IActionResult> GetFuels()
        {
            List<Fuel> Fuels = await _dbContext.Fuels.ToListAsync();
            return Ok(Fuels);
        }
    }
}
