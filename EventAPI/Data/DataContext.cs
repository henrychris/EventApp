using Microsoft.EntityFrameworkCore;
using Shared;

namespace EventAPI.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlite(_configuration.GetConnectionString("SqlConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasData
                (
                new Event
                {
                    Id = 1,
                    Name = "Summer Music Festival",
                    Description = "Join us for a day of live music and fun in the sun!",
                    Price = 50.00m,
                    Date = new DateTime(2023, 7, 15)
                },
                new Event
                {
                    Id = 2,
                    Name = "Summer Music Fest",
                    Description = "Join us for a day of live music and fun in the sun!",
                    Price = 45.00m,
                    Date = new DateTime(2023, 7, 22)
                },
                new Event
                {
                    Id = 3,
                    Name = "Summer Music Concert",
                    Description = "Join us for a night of live music and fun under the stars!",
                    Price = 55.00m,
                    Date = new DateTime(2023, 7, 29)
                });
        }
        public DbSet<Event> Events { get; set; }
    }
}
