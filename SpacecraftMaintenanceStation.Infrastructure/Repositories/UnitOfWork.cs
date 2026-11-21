using SpacecraftMaintenanceStation.Application.Abstractions;
using SpacecraftMaintenanceStation.Infrastructure.Persistence;

namespace SpacecraftMaintenanceStation.Infrastructure.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly StationDbContext _context;
        public IBayRepository Bays {  get; }
        public IMaintenanceRequestRepository MaintenanceRequests { get; }

        public UnitOfWork(StationDbContext context)
        {
            _context = context;
            MaintenanceRequests = new MaintenanceRequestRepository(context);
            Bays = new BayRepository(context);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
