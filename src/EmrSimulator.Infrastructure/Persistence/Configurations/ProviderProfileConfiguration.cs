using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class SyntheticCredentialSetConfiguration : IEntityTypeConfiguration<SyntheticCredentialSet>
{
    public void Configure(EntityTypeBuilder<SyntheticCredentialSet> builder)
    {
        builder.ToTable("SyntheticCredentialSets");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CredentialName).IsRequired().HasMaxLength(200);
        builder.Property(e => e.ClientId).HasMaxLength(200);
        builder.Property(e => e.ClientSecretHashOrMarker).HasMaxLength(300);
        builder.Property(e => e.Username).HasMaxLength(200);
        builder.Property(e => e.PasswordHashOrMarker).HasMaxLength(300);
        builder.Property(e => e.BearerToken).HasMaxLength(300);
        builder.Property(e => e.BasicAuthUser).HasMaxLength(200);
        builder.Property(e => e.BasicAuthPasswordHashOrMarker).HasMaxLength(300);
        builder.Property(e => e.TenantId).HasMaxLength(200);
        builder.Property(e => e.TenantShortName).HasMaxLength(100);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtUtc).IsRequired();

        builder.HasOne<EmrProfile>()
            .WithMany()
            .HasForeignKey(e => e.EmrProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}