using EmrSimulator.Contracts;
using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class EmrProfileConfiguration : IEntityTypeConfiguration<EmrProfile>
{
    public void Configure(EntityTypeBuilder<EmrProfile> builder)
    {
        builder.ToTable("EmrProfiles");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Provider)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<EmrProviderType>(v));

        builder.Property(e => e.BaseUrl)
            .HasMaxLength(500);

        builder.Property(e => e.NativeBaseUrl)
            .HasMaxLength(500);

        builder.Property(e => e.Hl7Host)
            .HasMaxLength(200);

        builder.Property(e => e.ResetGeneration)
            .HasDefaultValue(0);

        builder.Property(e => e.Enabled)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedAtUtc)
            .IsRequired();

        builder.Property(e => e.UpdatedAtUtc)
            .IsRequired();

        builder.HasMany<Scenario>()
            .WithOne()
            .HasForeignKey(s => s.EmrProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
