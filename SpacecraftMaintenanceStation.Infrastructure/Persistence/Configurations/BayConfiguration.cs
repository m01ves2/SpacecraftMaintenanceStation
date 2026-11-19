using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpacecraftMaintenanceStation.Domain.Entities;

namespace SpacecraftMaintenanceStation.Infrastructure.Persistence.Configurations
{
    internal class BayConfiguration : IEntityTypeConfiguration<Bay>
    {
        public void Configure(EntityTypeBuilder<Bay> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
             .IsRequired()
             .HasMaxLength(Bay.MaxNameLength);
        }
    }
}
