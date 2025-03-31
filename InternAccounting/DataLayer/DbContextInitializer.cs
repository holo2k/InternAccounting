using InternAccounting.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;

namespace InternAccounting.DataLayer
{
    public static class DbContextInitializer
    {

        public static void AddAppDbContext(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                   options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        }

        public static async void InitializeDbContext(AppDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
            dbContext.Directions.Add(new Entities.DirectionEntity {Title = "Разработка"});
            Console.WriteLine(dbContext.Interns);
        }
    }
}
