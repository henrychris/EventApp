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
                    Id = "ba0331a0-c649-4fab-9636-7fb4bbfcf396",
                    Name = "Summer Music Festival",
                    Description = "Join us for a day of live music and fun in the sun!",
                    Price = 50.00m,
                    Date = new DateTime(2023, 7, 15)
                },
                new Event
                {
                    Id = "03a5c81c-dc40-40b8-aa7a-1a2dff92ad67",
                    Name = "Summer Music Fest",
                    Description = "Join us for a day of live music and fun in the sun!",
                    Price = 45.00m,
                    Date = new DateTime(2023, 7, 22)
                },
                new Event
                {
                    Id = "61c904c7-fdb2-4f56-859e-e6a5151af515",
                    Name = "Summer Music Concert",
                    Description = "Join us for a night of live music and fun under the stars!",
                    Price = 55.00m,
                    Date = new DateTime(2023, 7, 29)
                });
        }
        public DbSet<Event> Events { get; set; }
    }
}
