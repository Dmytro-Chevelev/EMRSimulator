using EmrSimulator.Contracts;
using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class ScenarioConfiguration : IEntityTypeConfiguration<Scenario>
{
    public void Configure(EntityTypeBuilder<Scenario> builder)
    {
        builder.ToTable("Scenarios");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.ScenarioType)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<ScenarioType>(v));

        builder.Property(s => s.Seed)
            .HasMaxLength(500);

        builder.Property(s => s.EmrProfileId)
            .IsRequired();

        builder.Property(s => s.CreatedAtUtc)
            .IsRequired();

        builder.Property(s => s.UpdatedAtUtc)
            .IsRequired();

        builder.HasMany<MockResponse>()
            .WithOne()
            .HasForeignKey(m => m.ScenarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
