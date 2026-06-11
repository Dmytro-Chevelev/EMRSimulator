namespace EmrSimulator.Contracts;

public sealed record SyntheticAuthResult(bool Authorized, string Scheme, string Outcome, string? Token = null);

public sealed record ContractValidationResult(bool IsValid, IReadOnlyList<string> Errors);

public sealed record ExternalEmrRequestLogDto(
    string Provider,
    string EndpointOrOperation,
    string Protocol,
    string Outcome,
    string? CorrelationId,
    string ValidationStatus,
    string AuthOutcome);