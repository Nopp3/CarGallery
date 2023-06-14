using CarGalleryAPI.Controllers.Models;
using Microsoft.EntityFrameworkCore;

namespace CarGalleryAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        //Core DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }

        //Other DbSets
        public DbSet<Body> Bodies { get; set; }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Brand> Brands { get; set; }

    }
}
