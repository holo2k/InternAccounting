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
            try
            {
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "42P07")
            {
                Console.WriteLine($"Table already exists: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
                throw;
            }
        }
    }
}
