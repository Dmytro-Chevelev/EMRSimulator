using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class MockResponseConfiguration : IEntityTypeConfiguration<MockResponse>
{
    public void Configure(EntityTypeBuilder<MockResponse> builder)
    {
        builder.ToTable("MockResponses");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.ScenarioId)
            .IsRequired();

        builder.Property(m => m.RouteKey)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(m => m.StatusCode)
            .IsRequired();

        builder.Property(m => m.Body)
            .IsRequired(false);

        builder.Property(m => m.HeadersJson)
            .IsRequired(false);

        builder.Property(m => m.DelayMs)
            .HasDefaultValue(0);

        builder.Property(m => m.CreatedAtUtc)
            .IsRequired();

        builder.Property(m => m.UpdatedAtUtc)
            .IsRequired();
    }
}
