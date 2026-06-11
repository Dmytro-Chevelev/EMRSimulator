namespace EmrSimulator.Domain;

public sealed class SyntheticPatientGraph : BaseEntity
{
    public Guid ScenarioId { get; set; }
    public string PatientId { get; set; } = string.Empty;
    public string ExternalPatientId { get; set; } = string.Empty;
    public string Mrn { get; set; } = string.Empty;
    public string ProviderSpecificIdentifiersJson { get; set; } = "{}";
    public string DemographicsJson { get; set; } = "{}";
    public string EncounterJson { get; set; } = "{}";
    public string ProviderJson { get; set; } = "{}";
    public string VitalsJson { get; set; } = "{}";
    public string FhirJson { get; set; } = "{}";
    public string UnityXml { get; set; } = string.Empty;
    public string AdtHl7Message { get; set; } = string.Empty;
}

public sealed class SyntheticReportState : BaseEntity
{
    public Guid ScenarioId { get; set; }
    public Guid EmrProfileId { get; set; }
    public Guid? PatientGraphId { get; set; }
    public string ReportId { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string Status { get; set; } = "Generated";
    public string ReportMetadataJson { get; set; } = "{}";
    public string ReportDataBase64 { get; set; } = string.Empty;
    public string PdfBase64 { get; set; } = string.Empty;
    public Guid? CreatedByEndpointContractId { get; set; }
    public Guid? UpdatedByEndpointContractId { get; set; }
}

public sealed class DeviceRegistrationState : BaseEntity
{
    public Guid ScenarioId { get; set; }
    public Guid EmrProfileId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string InstanceId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public bool Connected { get; set; }
    public DateTime? LastHeartbeatAtUtc { get; set; }
    public string ActiveWorkflowJson { get; set; } = "{}";
    public string CalibrationStateJson { get; set; } = "{}";
}

public sealed class DocumentState : BaseEntity
{
    public Guid ScenarioId { get; set; }
    public Guid EmrProfileId { get; set; }
    public Guid? PatientGraphId { get; set; }
    public string AccessionNumber { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentMetadataXml { get; set; } = string.Empty;
    public string DocumentImageBase64 { get; set; } = string.Empty;
    public string SourceOperation { get; set; } = string.Empty;
}