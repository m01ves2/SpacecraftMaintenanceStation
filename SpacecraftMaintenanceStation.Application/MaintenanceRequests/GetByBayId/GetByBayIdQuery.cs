using SpacecraftMaintenanceStation.Application.Abstractions.Messaging;
using SpacecraftMaintenanceStation.Application.Common.Models;
using SpacecraftMaintenanceStation.Application.MaintenanceRequests.DTO;

namespace SpacecraftMaintenanceStation.Application.MaintenanceRequests.GetByBayId
{
    public sealed record GetByBayIdQuery(int BayId, DateTimeOffset StartTime, DateTimeOffset EndTime) : IQuery<Result<IReadOnlyList<MaintenanceRequestDTO>>>;
}
