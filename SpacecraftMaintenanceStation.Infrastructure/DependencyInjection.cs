using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpacecraftMaintenanceStation.Application.Abstractions.Persistence;
using SpacecraftMaintenanceStation.Infrastructure.Persistence;
using SpacecraftMaintenanceStation.Infrastructure.Repositories;

namespace SpacecraftMaintenanceStation.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<StationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            //services.AddScoped<IUnitOfWork>(options => options.GetRequiredService<StationDbContext>());

            // Repositories
            //services.AddScoped<IBayRepository, BayRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            // DateTimeProvider
            return services;
        }

    }
}
