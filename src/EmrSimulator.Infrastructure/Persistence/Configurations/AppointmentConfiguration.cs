using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.PatientId)
            .IsRequired();

        builder.Property(a => a.ProviderName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Status)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.StartTimeUtc)
            .IsRequired();

        builder.Property(a => a.EndTimeUtc)
            .IsRequired();

        builder.Property(a => a.CreatedAtUtc)
            .IsRequired();

        builder.Property(a => a.UpdatedAtUtc)
            .IsRequired();
    }
}
