using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class ResultConfiguration : IEntityTypeConfiguration<Result>
{
    public void Configure(EntityTypeBuilder<Result> builder)
    {
        builder.ToTable("Results");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.PatientId)
            .IsRequired();

        builder.Property(r => r.OrderId)
            .IsRequired(false);

        builder.Property(r => r.ResultType)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.Value)
            .IsRequired();

        builder.Property(r => r.ResultedAtUtc)
            .IsRequired();

        builder.Property(r => r.CreatedAtUtc)
            .IsRequired();

        builder.Property(r => r.UpdatedAtUtc)
            .IsRequired();
    }
}
