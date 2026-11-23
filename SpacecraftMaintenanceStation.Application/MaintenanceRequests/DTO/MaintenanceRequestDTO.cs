namespace SpacecraftMaintenanceStation.Application.MaintenanceRequests.DTO
{
    public sealed record MaintenanceRequestDTO(int BayId,
            string? SpaceCraftRegistrationNumber,
            string CustomerName,
            string ContactPhone,
            DateTimeOffset StartTime,
            DateTimeOffset EndTime,
            string ProblemDescription,
            string WorkDescription);
}
