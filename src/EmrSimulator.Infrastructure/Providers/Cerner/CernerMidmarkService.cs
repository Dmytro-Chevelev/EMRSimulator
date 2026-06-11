using EmrSimulator.Contracts.Cerner;
using EmrSimulator.Domain;
using EmrSimulator.Infrastructure.Persistence;

namespace EmrSimulator.Infrastructure.Providers.Cerner;

public sealed class CernerMidmarkService(EmrSimulatorDbContext dbContext)
{
    public IReadOnlyList<CernerMidmarkPatientResponse> SearchPatients() => dbContext.Patients
        .OrderBy(patient => patient.LastName)
        .ThenBy(patient => patient.FirstName)
        .Select(ToPatientResponse)
        .ToList();

    public CernerMidmarkPatientResponse? Patient(string id)
    {
        var databaseId = Guid.TryParse(id, out var parsedId) ? parsedId : (Guid?)null;
        var patient = dbContext.Patients
            .OrderBy(patient => patient.LastName)
            .ThenBy(patient => patient.FirstName)
            .FirstOrDefault(patient => patient.ExternalPatientId == id || patient.Mrn == id || (databaseId.HasValue && patient.Id == databaseId.Value));

        return patient is null ? null : ToPatientResponse(patient);
    }

    public IReadOnlyList<CernerPhysicianResponse> Physicians() =>
    [
        new("PHY-1001", "Dr. Avery", true)
    ];

    public CernerHl7SubmissionResponse Hl7Submitted(string? id = null) => new(id ?? "HL7-1001", "Accepted");

    private static CernerMidmarkPatientResponse ToPatientResponse(Patient patient) => new(
        patient.ExternalPatientId,
        patient.Id,
        patient.Mrn,
        patient.FirstName,
        patient.LastName,
        $"{patient.FirstName} {patient.LastName}",
        patient.DateOfBirth,
        patient.Gender,
        patient.Phone,
        patient.Email);
}