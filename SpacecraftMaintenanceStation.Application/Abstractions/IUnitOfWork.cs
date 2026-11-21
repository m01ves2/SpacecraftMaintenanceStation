namespace SpacecraftMaintenanceStation.Application.Abstractions
{
    public interface IUnitOfWork
    {
        IBayRepository Bays { get; }
        IMaintenanceRequestRepository MaintenanceRequests { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
