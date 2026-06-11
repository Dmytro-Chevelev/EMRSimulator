using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Application.Providers.Epic;

public interface IEpicSimulatorService
{
    EpicLaunchResponse Launch(string? launchToken, string? issuer);
    EpicTokenResponse Token();
    EpicFhirResourceResponse FhirResource(string resource);
    EpicReportResponse Report(string? reportId = null);
    EpicDeviceWorkflowResponse Device(string status);
}