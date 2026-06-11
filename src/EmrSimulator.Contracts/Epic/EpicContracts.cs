namespace EmrSimulator.Contracts.Epic;

public sealed record EpicTokenResponse(string AccessToken, string TokenType, int ExpiresIn, string Scope, string Patient);

public sealed record EpicLaunchResponse(string SessionId, string LaunchToken, string Issuer, string Status);

public sealed record EpicCloseResponse(string Status);

public sealed record EpicFhirCapabilityRestResponse(string Mode);

public sealed record EpicFhirCapabilityResponse(string ResourceType, string Status, IReadOnlyList<EpicFhirCapabilityRestResponse> Rest);

public sealed record EpicFhirResourceResponse(string ResourceType, string Id, string Patient, string Status);

public sealed record EpicReportResponse(string ReportId, string PatientId, string ReportType, string Status, string PdfBase64);

public sealed record EpicReportDataFileResponse(string FileId, string Status);

public sealed record EpicReportComparisonResponse(string ComparisonId, string Status);

public sealed record EpicDeviceWorkflowResponse(string DeviceId, string WorkflowId, string Status);

public sealed record EpicAuthenticationResponse(string Status, string Scheme);

public sealed record EpicLauncherRegistrationResponse(string LauncherId, string Status);

public sealed record EpicVerificationRecordResponse(string Provider, string Route, bool Verified);

public static class EpicSampleBuilder
{
    public static EpicLaunchResponse Launch(string? launchToken, string? issuer)
        => new(Guid.NewGuid().ToString("N"), launchToken ?? "synthetic-launch", issuer ?? "http://localhost:5288/FHIR/R4", "Launched");

    public static EpicTokenResponse Token()
        => new("synthetic-epic-token", "Bearer", 3600, "patient/*.read launch", "EP-1001");

    public static EpicReportResponse Report(string reportId = "RPT-1001")
        => new(reportId, "EP-1001", "ECG", "Available", Convert.ToBase64String("synthetic pdf"u8.ToArray()));

    public static EpicDeviceWorkflowResponse Device(string status)
        => new("MM-DEVICE-001", Guid.NewGuid().ToString("N"), status);
}