using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Infrastructure.Providers.Epic;

public sealed class EpicFhirService(EpicLaunchOAuthService inner)
{
    public EpicFhirCapabilityResponse Metadata()
        => new("CapabilityStatement", "active", [new EpicFhirCapabilityRestResponse("server")]);

    public EpicFhirResourceResponse Resource(string resource) => inner.FhirResource(resource);
}