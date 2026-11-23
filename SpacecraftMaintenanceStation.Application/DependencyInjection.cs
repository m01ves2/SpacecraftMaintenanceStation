using Microsoft.Extensions.DependencyInjection;
using SpacecraftMaintenanceStation.Application.Abstractions.Messaging;
using SpacecraftMaintenanceStation.Application.Bays.DTO;
using SpacecraftMaintenanceStation.Application.Bays.GetAll;
using SpacecraftMaintenanceStation.Application.Common.Models;
using SpacecraftMaintenanceStation.Application.MaintenanceRequests.Add;
using SpacecraftMaintenanceStation.Application.MaintenanceRequests.DTO;
using SpacecraftMaintenanceStation.Application.MaintenanceRequests.GetByBayId;

namespace SpacecraftMaintenanceStation.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IQueryHandler<GetAllQuery, Result<IReadOnlyList<BayDTO>>>, GetAllQueryHandler>();
            
            services.AddScoped<IQueryHandler<GetByBayIdQuery, Result<IReadOnlyList<MaintenanceRequestDTO>>>, GetByBayIdQueryHandler>();

            services.AddScoped<ICommandHandler<AddCommand, Result>, AddCommandHandler>();

            return services;
        }
    }
}
