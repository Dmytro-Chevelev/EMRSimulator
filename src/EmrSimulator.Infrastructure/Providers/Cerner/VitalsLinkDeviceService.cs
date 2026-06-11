using EmrSimulator.Contracts.Cerner;

namespace EmrSimulator.Infrastructure.Providers.Cerner;

public sealed class VitalsLinkDeviceService
{
    public object Register() => CernerSampleBuilder.Device("Registered");

    public object Heartbeat() => CernerSampleBuilder.Device("HeartbeatRecorded");

    public object PostVitals() => new { status = "Charted", patientId = "CE-1001", observationId = "OBS-1001" };

    public object Remove(string deviceId, string instanceId) => new { deviceId, instanceId, status = "Removed" };
}