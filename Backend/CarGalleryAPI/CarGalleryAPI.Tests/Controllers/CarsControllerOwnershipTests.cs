using CarGalleryAPI.Controllers;
using CarGalleryAPI.Controllers.Models;
using CarGalleryAPI.Data;
using CarGalleryAPI.Tests.TestSupport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Json;
using Xunit;

namespace CarGalleryAPI.Tests.Controllers
{
    public class CarsControllerOwnershipTests
    {
        [Fact]
        public async Task DeleteCar_WhenUserIsNotOwnerAndNotAdmin_ReturnsForbid()
        {
            var ownerId = Guid.NewGuid();
            var intruderId = Guid.NewGuid();
            var carId = Guid.NewGuid();

            await using var db = CreateDbContext();
            db.Cars.Add(new Car
            {
                id = carId,
                user_id = ownerId,
                fuel_id = 1,
                body_id = 1,
                brand_id = 1,
                model = "A4",
                productionYear = 2015,
                engine = 2000,
                horsePower = 190,
                imagePath = "/images/test.png"
            });
            await db.SaveChangesAsync();

            var controller = CreateController(db, CreateUser(intruderId, Roles.User));

            var result = await controller.DeleteCar(carId);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteCar_WhenUserIsOwner_ReturnsOk()
        {
            var ownerId = Guid.NewGuid();
            var carId = Guid.NewGuid();

            await using var db = CreateDbContext();
            db.Cars.Add(new Car
            {
                id = carId,
                user_id = ownerId,
                fuel_id = 1,
                body_id = 1,
                brand_id = 1,
                model = "M3",
                productionYear = 2020,
                engine = 3000,
                horsePower = 480,
                imagePath = "/images/test.png"
            });
            await db.SaveChangesAsync();

            var controller = CreateController(db, CreateUser(ownerId, Roles.User));

            var result = await controller.DeleteCar(carId);

            Assert.IsType<OkObjectResult>(result);
            Assert.Null(await db.Cars.FindAsync(carId));
        }

        [Fact]
        public async Task DeleteCar_WhenUserIsAdmin_ReturnsOkForForeignCar()
        {
            var ownerId = Guid.NewGuid();
            var adminId = Guid.NewGuid();
            var carId = Guid.NewGuid();

            await using var db = CreateDbContext();
            db.Cars.Add(new Car
            {
                id = carId,
                user_id = ownerId,
                fuel_id = 1,
                body_id = 1,
                brand_id = 1,
                model = "Supra",
                productionYear = 1998,
                engine = 3000,
                horsePower = 330,
                imagePath = "/images/test.png"
            });
            await db.SaveChangesAsync();

            var controller = CreateController(db, CreateUser(adminId, Roles.HeadAdmin));

            var result = await controller.DeleteCar(carId);

            Assert.IsType<OkObjectResult>(result);
            Assert.Null(await db.Cars.FindAsync(carId));
        }

        [Fact]
        public async Task UpdateCar_WhenUserIsNotOwnerAndNotAdmin_ReturnsForbid()
        {
            var ownerId = Guid.NewGuid();
            var intruderId = Guid.NewGuid();
            var carId = Guid.NewGuid();

            await using var db = CreateDbContext();
            db.Cars.Add(new Car
            {
                id = carId,
                user_id = ownerId,
                fuel_id = 1,
                body_id = 1,
                brand_id = 1,
                model = "OldModel",
                productionYear = 2010,
                engine = 1800,
                horsePower = 140,
                imagePath = "/images/test.png"
            });
            await db.SaveChangesAsync();

            var controller = CreateController(db, CreateUser(intruderId, Roles.User));

            var updatePayload = new Car
            {
                id = carId,
                user_id = ownerId,
                fuel_id = 1,
                body_id = 1,
                brand_id = 1,
                model = "NewModel",
                productionYear = 2012,
                engine = 2000,
                horsePower = 160,
                imagePath = "/images/test.png"
            };

            SetForm(controller, "updateCarRequest", JsonSerializer.Serialize(updatePayload));

            var result = await controller.UpdateCar(carId);

            Assert.IsType<ForbidResult>(result);

            var unchanged = await db.Cars.FindAsync(carId);
            Assert.NotNull(unchanged);
            Assert.Equal("OldModel", unchanged!.model);
        }

        private static DatabaseContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DatabaseContext(options);
        }

        private static CarsController CreateController(DatabaseContext db, ClaimsPrincipal user)
        {
            var env = new TestWebHostEnvironment();
            var config = new ConfigurationBuilder().Build();

            return new CarsController(db, env, config)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = user
                    }
                }
            };
        }

        private static ClaimsPrincipal CreateUser(Guid userId, Roles role)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role.ToString())
            }, "TestAuth"));
        }

        private static void SetForm(CarsController controller, string key, string value)
        {
            var formData = new Dictionary<string, StringValues> { [key] = value };
            var form = new FormCollection(formData);
            controller.ControllerContext.HttpContext.Features.Set<IFormFeature>(new FormFeature(form));
        }
    }
}
