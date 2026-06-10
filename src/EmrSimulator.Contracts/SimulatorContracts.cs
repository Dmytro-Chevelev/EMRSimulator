namespace EmrSimulator.Contracts;

public enum EmrProviderType
{
    Epic,
    Cerner,
    Altera,
    AthenaFlow,
    AthenaServer
}

public enum ScenarioType
{
    HappyPath,
    PatientNotFound,
    InvalidCredentials,
    Unauthorized,
    Timeout,
    ServerError,
    RateLimited,
    MalformedResponse
}

public sealed record ProviderSelectionDto(string ActiveProvider, string Message);

public sealed record ProviderRouteResult(int StatusCode, string Provider, string Route, object? Payload, string? Error = null);

public sealed record PatientDto(
    Guid Id,
    string ExternalPatientId,
    string Mrn,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Gender,
    string? Phone,
    string? Email);

public sealed record AppointmentDto(
    Guid Id,
    Guid PatientId,
    DateTime StartTimeUtc,
    DateTime EndTimeUtc,
    string ProviderName,
    string Status);

public sealed record OrderDto(
    Guid Id,
    Guid PatientId,
    string OrderType,
    string Status,
    DateTime PlacedAtUtc);

public sealed record ResultDto(
    Guid Id,
    Guid PatientId,
    Guid? OrderId,
    string ResultType,
    string Value,
    DateTime ResultedAtUtc);

public sealed record ScenarioDto(
    Guid Id,
    string Name,
    ScenarioType ScenarioType,
    bool IsActive,
    string Seed);

public sealed record RequestLogDto(
    Guid Id,
    string Provider,
    string Route,
    string Method,
    string RequestHeadersJson,
    string? RequestBody,
    string? ResponseBody,
    int ResponseCode,
    int DurationMs,
    Guid? ScenarioId,
    DateTime CreatedAtUtc);

public sealed record ImportRowResult(int RowNumber, bool Accepted, string? Reason, PatientDto? Patient);

public sealed record ImportReport(string SourceFormat, int AcceptedCount, int RejectedCount, IReadOnlyList<ImportRowResult> Rows);
