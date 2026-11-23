using SpacecraftMaintenanceStation.Domain.Entities;

namespace SpacecraftMaintenanceStation.Application.Abstractions.Persistence
{
    public interface IBayRepository
    {
        Task<IReadOnlyList<Bay>> GetAllAsync(CancellationToken cancellationToken);
        Task<Bay?> GetById(int id, CancellationToken cancellationToken);
    }
}
