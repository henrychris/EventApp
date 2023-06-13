using Microsoft.EntityFrameworkCore;
using Shared;

namespace UserAPI.Data
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
            modelBuilder.Entity<Role>().HasData
                (new Role
                {
                    Id = 1,
                    Name = UserRoles.Admin.ToString()
                },
                new Role
                {
                    Id = 2,
                    Name = UserRoles.User.ToString()
                });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
