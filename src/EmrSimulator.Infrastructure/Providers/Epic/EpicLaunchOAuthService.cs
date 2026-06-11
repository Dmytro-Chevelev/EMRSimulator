using EmrSimulator.Application.Providers.Epic;
using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Infrastructure.Providers.Epic;

public sealed class EpicLaunchOAuthService : IEpicSimulatorService
{
    public object Launch(string? launchToken, string? issuer) => EpicSampleBuilder.Launch(launchToken, issuer);

    public object Token() => EpicSampleBuilder.Token();

    public object FhirResource(string resource)
        => new
        {
            resourceType = ResolveResourceType(resource),
            id = resource.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? "EP-1001",
            patient = "EP-1001",
            status = "synthetic"
        };

    public object Report(string? reportId = null) => EpicSampleBuilder.Report(reportId ?? "RPT-1001");

    public object Device(string status) => EpicSampleBuilder.Device(status);

    private static string ResolveResourceType(string resource)
    {
        var segment = resource.Split('/', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        return string.IsNullOrWhiteSpace(segment) ? "Bundle" : segment;
    }
}