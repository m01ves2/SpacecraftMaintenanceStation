using Microsoft.EntityFrameworkCore;
using SpacecraftMaintenanceStation.Domain.Entities;

namespace SpacecraftMaintenanceStation.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(StationDbContext context)
        {
            var count = await context.Bays.CountAsync();
            if (count < 3) {
                var missingCount = 3 - count;

                var bays = Enumerable.Range(1, missingCount).Select(i => new Bay($"Bay-{Guid.NewGuid().ToString("N")[..6]}"));

                context.Bays.AddRange(bays);
                await context.SaveChangesAsync();
            }

            if (!await context.MaintenanceRequests.AnyAsync()) {

                var bays = await context.Bays
                                    .OrderBy(c => c.Id)
                                    .ToListAsync();

                var timeNow = DateTimeOffset.UtcNow;

                var maintenanceRequests = new[]
                {
                    new MaintenanceRequest(bays[0], "RN-123", "Davy", "123-45-67", timeNow.AddDays(1), timeNow.AddDays(1).AddHours(2), "some noice in muffler"),
                    new MaintenanceRequest(bays[1], "RN-235", "Bob Marley", "347-45-11", timeNow.AddDays(2), timeNow.AddDays(2).AddHours(6), "check engine error"),
                    new MaintenanceRequest(bays[1], "RN-350", "Fred", "148-00-02", timeNow.AddDays(1), timeNow.AddDays(1).AddHours(10), "brakes maintenance need"),
                    new MaintenanceRequest(bays[2], "RN-003", "Sindy", "3-45-71", timeNow.AddDays(4), timeNow.AddDays(4).AddHours(2), "forsage system unavailable"),
                };

                await context.MaintenanceRequests.AddRangeAsync(maintenanceRequests);
                await context.SaveChangesAsync();
            }
        }
    }
}
