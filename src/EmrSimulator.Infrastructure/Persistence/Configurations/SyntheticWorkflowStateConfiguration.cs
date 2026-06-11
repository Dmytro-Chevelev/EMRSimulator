using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmrSimulator.Infrastructure.Persistence.Configurations;

public sealed class SyntheticPatientGraphConfiguration : IEntityTypeConfiguration<SyntheticPatientGraph>
{
    public void Configure(EntityTypeBuilder<SyntheticPatientGraph> builder)
    {
        builder.ToTable("SyntheticPatientGraphs");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.PatientId).IsRequired().HasMaxLength(100);
        builder.Property(e => e.ExternalPatientId).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Mrn).IsRequired().HasMaxLength(100);
        builder.Property(e => e.ProviderSpecificIdentifiersJson).IsRequired();
        builder.Property(e => e.DemographicsJson).IsRequired();
        builder.Property(e => e.EncounterJson).IsRequired();
        builder.Property(e => e.ProviderJson).IsRequired();
        builder.Property(e => e.VitalsJson).IsRequired();
        builder.Property(e => e.FhirJson).IsRequired();
    }
}

public sealed class SyntheticReportStateConfiguration : IEntityTypeConfiguration<SyntheticReportState>
{
    public void Configure(EntityTypeBuilder<SyntheticReportState> builder)
    {
        builder.ToTable("SyntheticReportStates");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ReportId).IsRequired().HasMaxLength(100);
        builder.Property(e => e.ReportType).HasMaxLength(100);
        builder.Property(e => e.DeviceId).HasMaxLength(100);
        builder.Property(e => e.Status).IsRequired().HasMaxLength(100);
        builder.Property(e => e.ReportMetadataJson).IsRequired();
    }
}

public sealed class DeviceRegistrationStateConfiguration : IEntityTypeConfiguration<DeviceRegistrationState>
{
    public void Configure(EntityTypeBuilder<DeviceRegistrationState> builder)
    {
        builder.ToTable("DeviceRegistrationStates");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.DeviceId).IsRequired().HasMaxLength(100);
        builder.Property(e => e.InstanceId).IsRequired().HasMaxLength(100);
        builder.Property(e => e.DisplayName).HasMaxLength(200);
        builder.Property(e => e.DeviceType).HasMaxLength(100);
        builder.Property(e => e.ActiveWorkflowJson).IsRequired();
        builder.Property(e => e.CalibrationStateJson).IsRequired();
    }
}

public sealed class DocumentStateConfiguration : IEntityTypeConfiguration<DocumentState>
{
    public void Configure(EntityTypeBuilder<DocumentState> builder)
    {
        builder.ToTable("DocumentStates");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.AccessionNumber).HasMaxLength(100);
        builder.Property(e => e.DocumentType).HasMaxLength(100);
        builder.Property(e => e.SourceOperation).HasMaxLength(200);
    }
}