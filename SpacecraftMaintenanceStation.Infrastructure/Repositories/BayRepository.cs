using Microsoft.EntityFrameworkCore;
using SpacecraftMaintenanceStation.Application.Abstractions.Persistence;
using SpacecraftMaintenanceStation.Domain.Entities;
using SpacecraftMaintenanceStation.Infrastructure.Persistence;

namespace SpacecraftMaintenanceStation.Infrastructure.Repositories
{
    internal class BayRepository : IBayRepository
    {
        private readonly StationDbContext _context;

        public BayRepository(StationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Bay>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Bays.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Bay?> GetById(int id, CancellationToken cancellationToken)
        {
            return await _context.Bays.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }
    }
}
