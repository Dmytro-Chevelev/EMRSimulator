using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class Hl7MessageStateConfiguration : IEntityTypeConfiguration<Hl7MessageState>
{
    public void Configure(EntityTypeBuilder<Hl7MessageState> builder)
    {
        builder.ToTable("Hl7MessageStates");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Direction).IsRequired().HasMaxLength(50).HasConversion<string>();
        builder.Property(e => e.MessageType).IsRequired().HasMaxLength(50);
        builder.Property(e => e.ControlId).HasMaxLength(100);
        builder.Property(e => e.PatientIdentifier).HasMaxLength(100);
        builder.Property(e => e.RawMessage).IsRequired();
        builder.Property(e => e.AckMessage).HasMaxLength(2000);
        builder.Property(e => e.ValidationStatus).IsRequired().HasMaxLength(100);
        builder.Property(e => e.FailureReason).HasMaxLength(1000);
    }
}

public sealed class VerificationEvidenceConfiguration : IEntityTypeConfiguration<VerificationEvidence>
{
    public void Configure(EntityTypeBuilder<VerificationEvidence> builder)
    {
        builder.ToTable("VerificationEvidence");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.VerificationName).IsRequired().HasMaxLength(300);
        builder.Property(e => e.RequestSampleReference).HasMaxLength(500);
        builder.Property(e => e.ExpectedOutcome).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.ActualStatus).IsRequired().HasMaxLength(100);
        builder.Property(e => e.ActualResponseSummary).HasMaxLength(2000);
        builder.Property(e => e.FailureReason).HasMaxLength(1000);
        builder.Property(e => e.ToolOrTestName).IsRequired().HasMaxLength(300);
    }
}