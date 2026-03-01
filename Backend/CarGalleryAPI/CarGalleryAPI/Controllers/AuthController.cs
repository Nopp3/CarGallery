using CarGalleryAPI.Auth;
using CarGalleryAPI.Controllers.Models;
using CarGalleryAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarGalleryAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly DatabaseContext _dbContext;
        private readonly JwtOptions _jwtOptions;
        private readonly IWebHostEnvironment _environment;
        private readonly LoginRateLimiter _loginRateLimiter;

        public AuthController(DatabaseContext dbContext, IOptions<JwtOptions> jwtOptions,
            IWebHostEnvironment environment, LoginRateLimiter loginRateLimiter)
        {
            _dbContext = dbContext;
            _jwtOptions = jwtOptions.Value;
            _environment = environment;
            _loginRateLimiter = loginRateLimiter;
        }

        private CookieOptions BuildAuthCookieOptions(DateTime? expiresAtUtc = null)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = !_environment.IsDevelopment(),
                SameSite = SameSiteMode.Lax,
                Expires = expiresAtUtc,
                Path = "/",
                IsEssential = true
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            if (loginRequest == null)
                return BadRequest();

            if (_loginRateLimiter.IsBlocked(loginRequest.username))
                return StatusCode(StatusCodes.Status429TooManyRequests, "Too many failed login attempts. Try again later.");

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.username == loginRequest.username);
            if (user == null)
            {
                _loginRateLimiter.RegisterFailure(loginRequest.username);
                return Unauthorized();
            }

            if (!Hash.VerifyPassword(loginRequest.password, user.password))
            {
                _loginRateLimiter.RegisterFailure(loginRequest.username);
                return Unauthorized();
            }

            _loginRateLimiter.Reset(loginRequest.username);

            var now = DateTime.UtcNow;
            var expiresAtUtc = now.AddMinutes(_jwtOptions.AccessTokenMinutes);

            var role = Enum.IsDefined(typeof(Roles), user.role_id) ? ((Roles)user.role_id).ToString() : Roles.User.ToString();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.username),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.Role, role),
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: now,
                expires: expiresAtUtc,
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            Response.Cookies.Append(JwtOptions.AccessTokenCookieName, accessToken, BuildAuthCookieOptions(expiresAtUtc));

            return Ok(new AuthResponse
            {
                accessToken = accessToken,
                tokenType = "Bearer",
                expiresAtUtc = expiresAtUtc,
                userId = user.id,
                username = user.username,
                role = role
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(JwtOptions.AccessTokenCookieName, BuildAuthCookieOptions());
            return Ok();
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            if (User?.Identity?.IsAuthenticated != true)
                return Ok(null);

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
                return Ok(null);
            if (string.IsNullOrWhiteSpace(role))
                return Ok(null);

            return Ok(new AuthUserResponse
            {
                userId = userId,
                username = username,
                role = role
            });
        }

        public class AuthUserResponse
        {
            public Guid userId { get; set; }
            public string username { get; set; }
            public string role { get; set; }
        }

        public class AuthResponse
        {
            public string accessToken { get; set; }
            public string tokenType { get; set; }
            public DateTime expiresAtUtc { get; set; }
            public Guid userId { get; set; }
            public string username { get; set; }
            public string role { get; set; }
        }
    }
}
