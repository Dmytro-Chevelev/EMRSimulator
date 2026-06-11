using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Infrastructure.Providers.Epic;

public sealed class EpicDeviceWorkflowService
{
    public EpicDeviceWorkflowResponse Start() => EpicSampleBuilder.Device("Started");

    public EpicDeviceWorkflowResponse Abort() => EpicSampleBuilder.Device("Aborted");

    public EpicLauncherRegistrationResponse RegisterLauncher() => new("synthetic-launcher", "Registered");
}