using Microsoft.EntityFrameworkCore;
using CarRentalSystemAPI.Models;

namespace CarRentalSystemAPI.Data
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
