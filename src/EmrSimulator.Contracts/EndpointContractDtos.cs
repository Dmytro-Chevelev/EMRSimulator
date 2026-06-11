namespace EmrSimulator.Contracts;

public sealed record EndpointContractDto(
    Guid Id,
    string ContractKey,
    EmrProviderType Provider,
    string ContractFamily,
    string Direction,
    string Protocol,
    string? Method,
    string? PathPattern,
    string? ActionName,
    string Purpose,
    string RequestContractName,
    string ResponseContractName,
    bool AuthRequired,
    string AcceptedSerializerVariants,
    string SupportStatus,
    string SourceDocument,
    string SourceAnchor);

public sealed record ProviderProfileDto(
    Guid Id,
    string Name,
    EmrProviderType Provider,
    bool Enabled,
    string BaseUrl,
    string NativeBaseUrl,
    string? Hl7Host,
    int? Hl7Port,
    int ResetGeneration);

public sealed record VerificationEvidenceDto(
    Guid Id,
    Guid EndpointContractId,
    Guid EmrProfileId,
    Guid? ScenarioId,
    string VerificationName,
    string ExpectedOutcome,
    string ActualStatus,
    string ActualResponseSummary,
    bool Passed,
    string? FailureReason,
    DateTime VerifiedAtUtc,
    string ToolOrTestName);

public sealed record SimulatorResetResult(int ResetGeneration, string Message);