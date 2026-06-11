using EmrSimulator.Contracts;
using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class EndpointContractConfiguration : IEntityTypeConfiguration<EndpointContract>
{
    public void Configure(EntityTypeBuilder<EndpointContract> builder)
    {
        builder.ToTable("EndpointContracts");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ContractKey)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(e => e.ContractKey)
            .IsUnique();

        builder.Property(e => e.Provider)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(v => v.ToString(), v => Enum.Parse<EmrProviderType>(v));

        builder.Property(e => e.ContractFamily)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(v => v.ToString(), v => Enum.Parse<EndpointContractFamily>(v));

        builder.Property(e => e.Direction)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(v => v.ToString(), v => Enum.Parse<EndpointDirection>(v));

        builder.Property(e => e.Protocol)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(v => v.ToString(), v => Enum.Parse<EndpointProtocol>(v));

        builder.Property(e => e.SupportStatus)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(v => v.ToString(), v => Enum.Parse<EndpointSupportStatus>(v));

        builder.Property(e => e.Method).HasMaxLength(20);
        builder.Property(e => e.PathPattern).HasMaxLength(500);
        builder.Property(e => e.ActionName).HasMaxLength(500);
        builder.Property(e => e.Purpose).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.RequestContractName).HasMaxLength(300);
        builder.Property(e => e.ResponseContractName).HasMaxLength(300);
        builder.Property(e => e.AcceptedSerializerVariants).HasMaxLength(1000);
        builder.Property(e => e.SourceDocument).IsRequired().HasMaxLength(500);
        builder.Property(e => e.SourceAnchor).HasMaxLength(500);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtUtc).IsRequired();
    }
}