namespace EmrSimulator.Domain;

public enum Hl7MessageDirection
{
    Inbound,
    Outbound
}

public sealed class Hl7MessageState : BaseEntity
{
    public Guid ScenarioId { get; set; }
    public Guid EmrProfileId { get; set; }
    public Hl7MessageDirection Direction { get; set; }
    public string MessageType { get; set; } = string.Empty;
    public string ControlId { get; set; } = string.Empty;
    public string PatientIdentifier { get; set; } = string.Empty;
    public string RawMessage { get; set; } = string.Empty;
    public string AckMessage { get; set; } = string.Empty;
    public string ValidationStatus { get; set; } = "Pending";
    public string? FailureReason { get; set; }
    public DateTime ReceivedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? SentAtUtc { get; set; }
}

public sealed class VerificationEvidence : BaseEntity
{
    public Guid EndpointContractId { get; set; }
    public Guid EmrProfileId { get; set; }
    public Guid? ScenarioId { get; set; }
    public string VerificationName { get; set; } = string.Empty;
    public string RequestSampleReference { get; set; } = string.Empty;
    public string ExpectedOutcome { get; set; } = string.Empty;
    public string ActualStatus { get; set; } = string.Empty;
    public string ActualResponseSummary { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string? FailureReason { get; set; }
    public DateTime VerifiedAtUtc { get; set; } = DateTime.UtcNow;
    public string ToolOrTestName { get; set; } = string.Empty;
}