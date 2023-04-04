using Microsoft.EntityFrameworkCore;
using Shared;

namespace UserEventsAPI
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

        public DbSet<UserEvents> UserEvents { get; set; }
    }
}
