using SpacecraftMaintenanceStation.Domain.Entities;

namespace SpacecraftMaintenanceStation.Application.Abstractions
{
    public interface IBayRepository
    {
        Task<IReadOnlyList<Bay>> GetAllAsync(CancellationToken cancellationToken);
    }
}
