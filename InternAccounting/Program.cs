using InternAccounting.BusinessLayer.Mapper;
using InternAccounting.BusinessLayer.Services.Abstractions;
using InternAccounting.BusinessLayer.Services.Implementations;
using InternAccounting.DAL;
using InternAccounting.DataLayer;
using InternAccounting.DataLayer.Repositories.Abstractions;
using InternAccounting.DataLayer.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services);

        DbContextInitializer.AddAppDbContext(builder);

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        using var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        DbContextInitializer.InitializeDbContext(appDbContext);

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddScoped<IDirectionsRepository, DirectionsRepository>();
        services.AddScoped<IProjectsRepository, ProjectsRepository>();
        services.AddScoped<IInternsRepository, InternsRepository>();

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IDirectionService,DirectionService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IInternService, InternService>();
    }
}