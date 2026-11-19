using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpacecraftMaintenanceStation.Domain.Entities;

namespace SpacecraftMaintenanceStation.Infrastructure.Persistence.Configurations
{
    internal class MaintenanceRequestConfiguration : IEntityTypeConfiguration<MaintenanceRequest>
    {
        public void Configure(EntityTypeBuilder<MaintenanceRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Bay)
                    .WithMany()
                    .HasForeignKey(x => x.BayId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.CustomerName)
                    .IsRequired()
                    .HasMaxLength(MaintenanceRequest.MaxCustomerNameLength);

            builder.Property(x => x.SpaceCraftRegistrationNumber)
                    .HasMaxLength(MaintenanceRequest.MaxSpaceCraftRegistrationNumberLength);

            builder.Property(x => x.ProblemDescription)
                    .IsRequired()
                    .HasMaxLength(MaintenanceRequest.MaxProblemDescriptionLength);

            builder.Property(x => x.WorkDescription)
                    .IsRequired()
                    .HasMaxLength(MaintenanceRequest.MaxWorkDescriptionLength);

            builder.Property(x => x.ContactPhone)
                    .IsRequired()
                    .HasMaxLength(MaintenanceRequest.MaxContactPhoneLength);

            builder.Property(x => x.Status)
                    .IsRequired();

            builder.Property(x => x.StartTime)
                    .IsRequired();

            builder.Property(x => x.EndTime)
                    .IsRequired();
        }
    }
}
