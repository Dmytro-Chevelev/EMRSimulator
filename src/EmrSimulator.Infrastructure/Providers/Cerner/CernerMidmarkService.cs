namespace EmrSimulator.Infrastructure.Providers.Cerner;

public sealed class CernerMidmarkService
{
    public object SearchPatients() => new[] { new { id = "ADT-1001", mrn = "MRN-1001", name = "Jordan Casey" } };

    public object Patient(string id) => new { id, mrn = "MRN-1001", firstName = "Jordan", lastName = "Casey" };

    public object Physicians() => new[] { new { id = "PHY-1001", displayName = "Dr. Avery", active = true } };

    public object Hl7Submitted(string? id = null) => new { messageId = id ?? "HL7-1001", status = "Accepted" };
}