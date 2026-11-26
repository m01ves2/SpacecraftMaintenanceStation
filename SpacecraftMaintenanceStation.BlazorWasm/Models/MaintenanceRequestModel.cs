namespace SpacecraftMaintenanceStation.BlazorWasm.Models
{    
    public sealed record MaintenanceRequestModel(int BayId,
            string? SpaceCraftRegistrationNumber,
            string CustomerName,
            string ContactPhone,
            DateTimeOffset StartTime,
            DateTimeOffset EndTime,
            string ProblemDescription,
            string WorkDescription);
}
