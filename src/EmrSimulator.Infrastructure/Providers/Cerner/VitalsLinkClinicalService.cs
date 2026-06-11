using EmrSimulator.Contracts.Cerner;

namespace EmrSimulator.Infrastructure.Providers.Cerner;

public sealed class VitalsLinkClinicalService
{
    public VitalsLinkBarcodeFormatsResponse BarcodeFormats() => new(["MRN", "FIN"]);

    public VitalsLinkPersonnelResponse Personnel(string barcode) => new(barcode, "PER-1001", "Synthetic Clinician");

    public IReadOnlyList<VitalsLinkLocationResponse> Locations() => [new("LOC-1001", "Synthetic Clinic")];

    public VitalsLinkEncounterResponse Encounter(string? encounterId = null) => new(encounterId ?? "ENC-1001", "CE-1001", "Active");

    public CernerPatientResponse Patient(string? patientId = null) => CernerSampleBuilder.Patient(patientId ?? "CE-1001");
}