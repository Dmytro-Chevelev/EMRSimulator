using EmrSimulator.Contracts.Cerner;

namespace EmrSimulator.Infrastructure.Providers.Cerner;

public sealed class VitalsLinkClinicalService
{
    public object BarcodeFormats() => new { formats = new[] { "MRN", "FIN" } };

    public object Personnel(string barcode) => new { barcode, personnelId = "PER-1001", displayName = "Synthetic Clinician" };

    public object Locations() => new[] { new { locationId = "LOC-1001", name = "Synthetic Clinic" } };

    public object Encounter(string? encounterId = null) => new { encounterId = encounterId ?? "ENC-1001", patientId = "CE-1001", status = "Active" };

    public object Patient(string? patientId = null) => CernerSampleBuilder.Patient(patientId ?? "CE-1001");
}