namespace SpacecraftMaintenanceStation.BlazorWasm.Models
{    
    public sealed record MaintenanceRequestModel(int BayId,
            string? SpaceCraftRegistrationNumber,
            string CustomerName,
            string ContactPhone,
            DateTimeOffset StartTime,
            DateTimeOffset EndTime,
            string ProblemDescription,
            string WorkDescription)
    {
        public DateTime LocalStartTime => StartTime.LocalDateTime;
        public DateTime LocalEndTime => EndTime.LocalDateTime;
        public string CalendarText => string.IsNullOrWhiteSpace(ContactPhone) ? CustomerName : $"{CustomerName} · {ContactPhone}";
    }
}
