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

        public AuthController(DatabaseContext dbContext, IOptions<JwtOptions> jwtOptions)
        {
            _dbContext = dbContext;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            if (loginRequest == null)
                return BadRequest();

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.username == loginRequest.username);
            if (user == null)
                return Unauthorized();

            if (!Hash.VerifyPassword(loginRequest.password, user.password))
                return Unauthorized();

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
            Response.Cookies.Append(JwtOptions.AccessTokenCookieName, accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = HttpContext.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = expiresAtUtc,
                Path = "/",
                IsEssential = true
            });

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
            Response.Cookies.Delete(JwtOptions.AccessTokenCookieName, new CookieOptions
            {
                HttpOnly = true,
                Secure = HttpContext.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                IsEssential = true
            });
            return Ok();
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
                return Unauthorized();
            if (string.IsNullOrWhiteSpace(role))
                return Unauthorized();

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
