using EmrSimulator.Application.Providers.Epic;
using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Infrastructure.Providers.Epic;

public sealed class EpicLaunchOAuthService : IEpicSimulatorService
{
    public EpicLaunchResponse Launch(string? launchToken, string? issuer) => EpicSampleBuilder.Launch(launchToken, issuer);

    public EpicTokenResponse Token() => EpicSampleBuilder.Token();

    public EpicFhirResourceResponse FhirResource(string resource)
        => new(
            ResolveResourceType(resource),
            resource.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? "EP-1001",
            "EP-1001",
            "synthetic");

    public EpicReportResponse Report(string? reportId = null) => EpicSampleBuilder.Report(reportId ?? "RPT-1001");

    public EpicDeviceWorkflowResponse Device(string status) => EpicSampleBuilder.Device(status);

    private static string ResolveResourceType(string resource)
    {
        var segment = resource.Split('/', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        return string.IsNullOrWhiteSpace(segment) ? "Bundle" : segment;
    }
}