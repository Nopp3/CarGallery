using CarGalleryAPI.Auth;
using CarGalleryAPI.Controllers;
using CarGalleryAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Xunit;

namespace CarGalleryAPI.Tests.Auth
{
    public class AuthControllerMeTests
    {
        [Fact]
        public void Me_WhenUnauthenticated_ReturnsOkWithNull()
        {
            var controller = CreateController();

            var result = controller.Me();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public void Me_WhenAuthenticatedWithValidClaims_ReturnsUserPayload()
        {
            var userId = Guid.NewGuid();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, "alice"),
                new Claim(ClaimTypes.Role, "HeadAdmin")
            }, "TestAuth"));

            var controller = CreateController(user);

            var result = controller.Me();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var payload = Assert.IsType<AuthController.AuthUserResponse>(okResult.Value);
            Assert.Equal(userId, payload.userId);
            Assert.Equal("alice", payload.username);
            Assert.Equal("HeadAdmin", payload.role);
        }

        [Fact]
        public void Me_WhenUserIdClaimIsInvalid_ReturnsOkWithNull()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "not-a-guid"),
                new Claim(ClaimTypes.Name, "alice"),
                new Claim(ClaimTypes.Role, "User")
            }, "TestAuth"));

            var controller = CreateController(user);

            var result = controller.Me();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Null(okResult.Value);
        }

        private static AuthController CreateController(ClaimsPrincipal? user = null)
        {
            var dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().Options;
            var dbContext = new DatabaseContext(dbContextOptions);

            var jwtOptions = Options.Create(new JwtOptions
            {
                SigningKey = "test-signing-key-at-least-32-chars!!",
                Issuer = "CarGallery",
                Audience = "CarGalleryClient",
                AccessTokenMinutes = 30
            });

            var environment = new TestWebHostEnvironment();
            var limiter = new LoginRateLimiter(new MemoryCache(new MemoryCacheOptions()));

            var controller = new AuthController(dbContext, jwtOptions, environment, limiter)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = user ?? new ClaimsPrincipal(new ClaimsIdentity())
                    }
                }
            };

            return controller;
        }

        private class TestWebHostEnvironment : IWebHostEnvironment
        {
            public string EnvironmentName { get; set; } = "Development";
            public string ApplicationName { get; set; } = "CarGalleryAPI.Tests";
            public string WebRootPath { get; set; } = string.Empty;
            public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
            public string ContentRootPath { get; set; } = string.Empty;
            public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        }
    }
}
