using SpacecraftMaintenanceStation.Application.Abstractions.Messaging;
using SpacecraftMaintenanceStation.Application.Common.Models;

namespace SpacecraftMaintenanceStation.Application.MaintenanceRequests.Add
{
    public sealed record AddCommand(int BayId,
        string? SpaceCraftRegistrationNumber,
        string CustomerName,
        string ContactPhone,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime,
        string ProblemDescription) : ICommand<Result>;
}
