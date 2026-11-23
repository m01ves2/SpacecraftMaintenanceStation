using Microsoft.EntityFrameworkCore;
using SpacecraftMaintenanceStation.Application.Abstractions.Persistence;
using SpacecraftMaintenanceStation.Domain.Entities;
using SpacecraftMaintenanceStation.Infrastructure.Persistence;

namespace SpacecraftMaintenanceStation.Infrastructure.Repositories
{
    internal class MaintenanceRequestRepository : IMaintenanceRequestRepository
    {
        private readonly StationDbContext _context;

        public MaintenanceRequestRepository(StationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MaintenanceRequest maintenanceRequest, CancellationToken cancellationToken)
        {
            await _context.MaintenanceRequests.AddAsync(maintenanceRequest, cancellationToken);
        }

        public async Task<IReadOnlyList<MaintenanceRequest>> GetByBayIdAsync(int bayId, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken)
        {
            return await _context.MaintenanceRequests
                                    .AsNoTracking()
                                    .Where(b => b.BayId == bayId && !(b.StartTime >= to || b.EndTime <= from))
                                    .ToListAsync(cancellationToken);
        }
    }
}
