using SpacecraftMaintenanceStation.Domain.Entities;

namespace SpacecraftMaintenanceStation.Application.Abstractions
{
    public interface IMaintenanceRequestRepository
    {
        Task<IReadOnlyList<MaintenanceRequest>> GetByBayIdAsync(int bayId, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken);
        Task AddAsync(MaintenanceRequest maintenanceRequest, CancellationToken cancellationToken);
    }
}
