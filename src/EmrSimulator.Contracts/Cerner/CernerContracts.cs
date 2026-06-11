namespace EmrSimulator.Contracts.Cerner;

public sealed record CernerAuthResponse(string SessionId, string Tenant, string Status);

public sealed record CernerPatientResponse(string PatientId, string EncounterId, string FirstName, string LastName, string Mrn);

public sealed record CernerDeviceResponse(string DeviceId, string InstanceId, string Status);

public sealed record VitalsLinkBarcodeFormatsResponse(IReadOnlyList<string> Formats);

public sealed record VitalsLinkPersonnelResponse(string Barcode, string PersonnelId, string DisplayName);

public sealed record VitalsLinkLocationResponse(string LocationId, string Name);

public sealed record VitalsLinkEncounterResponse(string EncounterId, string PatientId, string Status);

public sealed record VitalsLinkChartingResponse(string Status, string PatientId, string ObservationId);

public sealed record VitalsLinkDeviceRemovalResponse(string DeviceId, string InstanceId, string Status);

public sealed record Hl7AckResponse(string ControlId, string AckCode, string Message);

public sealed record CernerMidmarkPatientResponse(
    string Id,
    Guid PatientId,
    string Mrn,
    string FirstName,
    string LastName,
    string Name,
    DateOnly DateOfBirth,
    string Gender,
    string? Phone,
    string? Email);

public sealed record CernerPhysicianResponse(string Id, string DisplayName, bool Active);

public sealed record CernerHl7SubmissionResponse(string MessageId, string Status);

public sealed record CernerLastAccessUpdateResponse(string Status);

public sealed record CernerVerificationRecordResponse(string Provider, string Route, bool Verified);

public static class CernerSampleBuilder
{
    public static CernerAuthResponse Auth() => new("synthetic-cerner-session", "synthetic-tenant", "Authenticated");

    public static CernerPatientResponse Patient(string patientId = "CE-1001") => new(patientId, "ENC-1001", "Jordan", "Casey", "MRN-1001");

    public static CernerDeviceResponse Device(string status) => new("MM-DEVICE-001", "INSTANCE-001", status);

    public static Hl7AckResponse Ack(string controlId, bool accepted) => new(controlId, accepted ? "AA" : "AE", accepted ? "Accepted" : "Rejected");
}