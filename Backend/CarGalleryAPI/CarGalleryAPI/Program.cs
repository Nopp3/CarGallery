using CarGalleryAPI.Auth;
using CarGalleryAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CarGalleryAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            DbCreator.Initialize(builder.Configuration);

            if (!DbCreator.DoesDbExist())
            {
                DbCreator.ValidateSeedAdminConfig();
                DbCreator.CreateDatabase();
            }

            // Add services to the container.
            builder.Services.AddDbContext<Data.DatabaseContext>(options =>
            {
                var server = builder.Configuration["DatabaseConnection:Server"];
                var useWindowsAuthentication = builder.Configuration.GetValue<bool>("DatabaseConnection:useWindowsAuthentication");
                if (useWindowsAuthentication)
                    options.UseSqlServer($"Server={server};Database=CarGalleryDB;Integrated Security=True;TrustServerCertificate=true;");
                else
                {
                    var username = builder.Configuration["DatabaseConnection:Username"];
                    var password = builder.Configuration["DatabaseConnection:Password"];
                    options.UseSqlServer($"Server={server};Database=CarGalleryDB;User Id={username};Password={password};TrustServerCertificate=true;");
                }
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            builder.Services.AddMvc();
            builder.Services.AddHealthChecks();

            var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
            builder.Services.Configure<JwtOptions>(jwtSection);
            var jwtOptions = jwtSection.Get<JwtOptions>() ?? new JwtOptions();

            if (string.IsNullOrWhiteSpace(jwtOptions.SigningKey) || jwtOptions.SigningKey.Length < 32)
                throw new InvalidOperationException("Jwt:SigningKey must be set and at least 32 characters. Set it via env var Jwt__SigningKey.");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (!string.IsNullOrWhiteSpace(context.Token))
                                return Task.CompletedTask;

                            if (context.Request.Headers.ContainsKey("Authorization"))
                                return Task.CompletedTask;

                            if (context.Request.Cookies.TryGetValue(JwtOptions.AccessTokenCookieName, out var cookieToken)
                                && !string.IsNullOrWhiteSpace(cookieToken))
                            {
                                context.Token = cookieToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1),
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RoleClaimType = ClaimTypes.Role
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseHttpsRedirection();
            
            app.UseStatusCodePages(async statusCodeContext =>
            {
                var httpContext = statusCodeContext.HttpContext;
                var response = httpContext.Response;

                if (response.StatusCode is not StatusCodes.Status401Unauthorized and not StatusCodes.Status403Forbidden)
                    return;

                if (!httpContext.Request.Path.StartsWithSegments("/api"))
                    return;

                if (response.HasStarted || !string.IsNullOrEmpty(response.ContentType))
                    return;

                var isUnauthorized = response.StatusCode == StatusCodes.Status401Unauthorized;

                var problemDetails = new ProblemDetails
                {
                    Title = isUnauthorized ? "Unauthorized" : "Forbidden",
                    Status = response.StatusCode,
                    Detail = isUnauthorized
                        ? "Authentication is required to access this resource."
                        : "You do not have permission to access this resource.",
                    Instance = httpContext.Request.Path
                };
                problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

                response.ContentType = "application/problem+json";
                await response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            });

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
