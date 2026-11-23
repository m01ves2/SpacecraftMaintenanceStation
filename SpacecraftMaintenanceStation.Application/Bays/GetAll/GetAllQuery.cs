using SpacecraftMaintenanceStation.Application.Abstractions.Messaging;
using SpacecraftMaintenanceStation.Application.Bays.DTO;
using SpacecraftMaintenanceStation.Application.Common.Models;

namespace SpacecraftMaintenanceStation.Application.Bays.GetAll
{
    public sealed record GetAllQuery : IQuery<Result<IReadOnlyList<BayDTO>>>;
}
