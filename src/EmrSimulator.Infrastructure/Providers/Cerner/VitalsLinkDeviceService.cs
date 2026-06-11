using EmrSimulator.Contracts.Cerner;

namespace EmrSimulator.Infrastructure.Providers.Cerner;

public sealed class VitalsLinkDeviceService
{
    public CernerDeviceResponse Register() => CernerSampleBuilder.Device("Registered");

    public CernerDeviceResponse Heartbeat() => CernerSampleBuilder.Device("HeartbeatRecorded");

    public VitalsLinkChartingResponse PostVitals() => new("Charted", "CE-1001", "OBS-1001");

    public VitalsLinkDeviceRemovalResponse Remove(string deviceId, string instanceId) => new(deviceId, instanceId, "Removed");
}