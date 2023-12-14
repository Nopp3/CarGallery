using CarGalleryAPI.Data;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddSwaggerGen();

            builder.Services.AddMvc();

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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}