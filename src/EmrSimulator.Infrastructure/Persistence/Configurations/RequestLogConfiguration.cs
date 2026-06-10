using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
{
    public void Configure(EntityTypeBuilder<RequestLog> builder)
    {
        builder.ToTable("RequestLogs");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Provider)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Route)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(r => r.Method)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(r => r.RequestHeadersJson)
            .IsRequired();

        builder.Property(r => r.RequestBody)
            .IsRequired(false);

        builder.Property(r => r.ResponseBody)
            .IsRequired(false);

        builder.Property(r => r.ResponseCode)
            .IsRequired();

        builder.Property(r => r.DurationMs)
            .IsRequired();

        builder.Property(r => r.ScenarioId)
            .IsRequired(false);

        builder.Property(r => r.CreatedAtUtc)
            .IsRequired();

        builder.Property(r => r.UpdatedAtUtc)
            .IsRequired();

        builder.HasOne<Scenario>()
            .WithMany()
            .HasForeignKey(r => r.ScenarioId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
