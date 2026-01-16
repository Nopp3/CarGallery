using CarGalleryAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CarGalleryAPI.Controllers.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CarGalleryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        private readonly DatabaseContext _dbContext;
        private readonly IWebHostEnvironment _env;
        public CarsController(DatabaseContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        [HttpGet("all")]
        public IActionResult GetCars()
        {
            #region LINQ query
            var query = from car in _dbContext.Cars
                        join user in _dbContext.Users on car.user_id equals user.id into userJoin
                        from user in userJoin.DefaultIfEmpty()
                        join fuel in _dbContext.Fuels on car.fuel_id equals fuel.id into fuelJoin
                        from fuel in fuelJoin.DefaultIfEmpty()
                        join body in _dbContext.Bodies on car.body_id equals body.id into bodyJoin
                        from body in bodyJoin.DefaultIfEmpty()
                        join brand in _dbContext.Brands on car.brand_id equals brand.id into brandJoin
                        from brand in brandJoin.DefaultIfEmpty()
                        select new
                        {
                            car.id,
                            //car.user_id,
                            user.username,
                            fuel = fuel.type,
                            body = body.type,
                            brand = brand.name,
                            car.model,
                            car.productionYear,
                            car.engine,
                            car.horsePower,
                            car.imagePath
                        };
            #endregion
            var result = query.ToList();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCar([FromQuery] Guid id)
        {
            Car car = await _dbContext.Cars.FindAsync(id);
            if (car == null)
                return NotFound("Not found car with id: " + id);
            return Ok(car);
        }

        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> GetUserCars([FromQuery] Guid id)
        {
            List<Car> Cars = await _dbContext.Cars.ToListAsync();
            #region LINQ query
            var query = from car in Cars.Where(x => x.user_id == id)
                        join user in _dbContext.Users on car.user_id equals user.id into userJoin
                        from user in userJoin.DefaultIfEmpty()
                        join fuel in _dbContext.Fuels on car.fuel_id equals fuel.id into fuelJoin
                        from fuel in fuelJoin.DefaultIfEmpty()
                        join body in _dbContext.Bodies on car.body_id equals body.id into bodyJoin
                        from body in bodyJoin.DefaultIfEmpty()
                        join brand in _dbContext.Brands on car.brand_id equals brand.id into brandJoin
                        from brand in brandJoin.DefaultIfEmpty()
                        select new
                        {
                            car.id,
                            //car.user_id,
                            user.username,
                            fuel = fuel.type,
                            body = body.type,
                            brand = brand.name,
                            car.model,
                            car.productionYear,
                            car.engine,
                            car.horsePower,
                            car.imagePath
                        };
            #endregion
            var result = query.ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("brand")]
        public async Task<IActionResult> GetCarsByBrand([FromQuery] int id)
        {
            Brand? brands = await _dbContext.Brands.FirstOrDefaultAsync(x => x.id == id);
            List<Car> cars = await _dbContext.Cars.ToListAsync();
            if (brands != null && cars.Where(x => x.brand_id == brands.id).Any())
            {
                #region LINQ query
                var query = from car in cars.Where(x => x.brand_id == brands.id)
                            join user in _dbContext.Users on car.user_id equals user.id into userJoin
                            from user in userJoin.DefaultIfEmpty()
                            join fuel in _dbContext.Fuels on car.fuel_id equals fuel.id into fuelJoin
                            from fuel in fuelJoin.DefaultIfEmpty()
                            join body in _dbContext.Bodies on car.body_id equals body.id into bodyJoin
                            from body in bodyJoin.DefaultIfEmpty()
                            select new
                            {
                                car.id,
                                //car.user_id,
                                user.username,
                                fuel = fuel.type,
                                body = body.type,
                                brand = brands.name,
                                car.model,
                                car.productionYear,
                                car.engine,
                                car.horsePower,
                                car.imagePath
                            };
                #endregion
                var result = query.ToList();
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddCar()
        {
            var formCollection = await Request.ReadFormAsync();

            //Upload File
            var file = formCollection.Files.GetFile("image");
            if (file == null || file.Length == 0)
                return BadRequest("TEST");

            string uniqueImgName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            string imagePath = Path.Combine(_env.WebRootPath, "images", uniqueImgName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            //Add Car
            var carJson = formCollection["carRequest"];
            var carRequest = JsonSerializer.Deserialize<Car>(carJson);
            carRequest.id = Guid.NewGuid();
            carRequest.imagePath = $@"/images/{uniqueImgName}";
            await _dbContext.AddAsync(carRequest);
            await _dbContext.SaveChangesAsync();

            return Ok(carRequest);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCar([FromRoute] Guid id)
        {
            var formCollection = await Request.ReadFormAsync();
            string updateImagePath = "";
            var file = formCollection.Files.GetFile("image");
            if (file != null && file.Length != 0)
            {
                string uniqueImgName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
                string imagePath = Path.Combine(_env.WebRootPath, "images", uniqueImgName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                updateImagePath = $@"/images/{uniqueImgName}";
            }

            var carJson = formCollection["updateCarRequest"];
            var updateCarRequest = JsonSerializer.Deserialize<Car>(carJson);
            var car = await _dbContext.Cars.FindAsync(id);

            if (car == null)
                return NotFound();
            if (updateImagePath == "")
            {
                updateImagePath = updateCarRequest.imagePath;
            }
            else
            {
                System.IO.File.Delete(_env.WebRootPath + updateCarRequest.imagePath.Replace('/', '\\'));
            }

            car.model = updateCarRequest.model;
            car.fuel_id = updateCarRequest.fuel_id;
            car.imagePath = updateImagePath;
            car.body_id = updateCarRequest.body_id;
            car.brand_id = updateCarRequest.brand_id;
            car.engine = updateCarRequest.engine;
            car.productionYear = updateCarRequest.productionYear;
            car.horsePower = updateCarRequest.horsePower;

            await _dbContext.SaveChangesAsync();
            return Ok(car);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCar([FromQuery] Guid id)
        {
            var car = await _dbContext.Cars.FindAsync(id);

            if (car == null)
                return NotFound();

            System.IO.File.Delete(_env.WebRootPath + car.imagePath.Replace('/', '\\'));

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
