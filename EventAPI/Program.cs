
using EventAPI.Interfaces;
using EventAPI.Services;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace EventAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region My Services

            builder.Services.Configure<AppSettings>(config.GetSection("AppSettings"));

            // Add and seed DB
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite());
            using var context = new DataContext(config);
            context.Database.EnsureCreated();

            // other services
            builder.Services.AddScoped<IEventRepository, EventRepository>();

            #endregion
            var app = builder.Build();

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