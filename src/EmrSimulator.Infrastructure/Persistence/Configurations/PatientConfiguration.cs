using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Mrn)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(p => p.Mrn)
            .IsUnique();

        builder.Property(p => p.ExternalPatientId)
            .HasMaxLength(200);

        builder.HasIndex(p => p.ExternalPatientId);

        builder.Property(p => p.Gender)
            .HasMaxLength(50);

        builder.Property(p => p.Phone)
            .HasMaxLength(50);

        builder.Property(p => p.Email)
            .HasMaxLength(250);

        builder.Property(p => p.CreatedAtUtc)
            .IsRequired();

        builder.Property(p => p.UpdatedAtUtc)
            .IsRequired();

        builder.HasMany<Appointment>()
            .WithOne()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<Order>()
            .WithOne()
            .HasForeignKey(o => o.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<Result>()
            .WithOne()
            .HasForeignKey(r => r.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
