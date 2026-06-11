using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.PatientId)
            .IsRequired();

        builder.Property(o => o.OrderType)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.Status)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.PlacedAtUtc)
            .IsRequired();

        builder.Property(o => o.CreatedAtUtc)
            .IsRequired();

        builder.Property(o => o.UpdatedAtUtc)
            .IsRequired();

        builder.HasMany<Result>()
            .WithOne()
            .HasForeignKey(r => r.OrderId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
