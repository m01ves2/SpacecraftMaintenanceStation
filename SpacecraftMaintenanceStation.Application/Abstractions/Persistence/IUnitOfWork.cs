namespace SpacecraftMaintenanceStation.Application.Abstractions.Persistence
{
    public interface IUnitOfWork
    {
        IBayRepository Bays { get; }
        IMaintenanceRequestRepository MaintenanceRequests { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
