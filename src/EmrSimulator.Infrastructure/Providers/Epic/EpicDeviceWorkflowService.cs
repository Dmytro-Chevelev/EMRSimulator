using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Infrastructure.Providers.Epic;

public sealed class EpicDeviceWorkflowService
{
    public object Start() => EpicSampleBuilder.Device("Started");

    public object Abort() => EpicSampleBuilder.Device("Aborted");

    public object RegisterLauncher() => new { launcherId = "synthetic-launcher", status = "Registered" };
}