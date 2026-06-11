using EmrSimulator.Application.Providers.Cerner;
using EmrSimulator.Contracts.Cerner;

namespace EmrSimulator.Infrastructure.Providers.Cerner;

public sealed class VitalsLinkAuthService : ICernerSimulatorService
{
    public CernerAuthResponse Auth() => CernerSampleBuilder.Auth();

    public CernerPatientResponse Patient(string? patientId = null) => CernerSampleBuilder.Patient(patientId ?? "CE-1001");

    public CernerDeviceResponse Device(string status) => CernerSampleBuilder.Device(status);

    public string Acknowledge(string message)
        => $"MSH|^~\\&|SIM|MIDMARK|CONNECTOR|LOCAL|{DateTime.UtcNow:yyyyMMddHHmmss}||ACK|ACK-1|P|2.5\rMSA|AA|{ExtractControlId(message)}";

    private static string ExtractControlId(string message)
    {
        var msh = message.Split('\r', '\n').FirstOrDefault(segment => segment.StartsWith("MSH", StringComparison.OrdinalIgnoreCase));
        var parts = msh?.Split('|');
        return parts is { Length: > 9 } ? parts[9] : "SYNTHETIC";
    }
}