﻿using Microsoft.EntityFrameworkCore;
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


        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
