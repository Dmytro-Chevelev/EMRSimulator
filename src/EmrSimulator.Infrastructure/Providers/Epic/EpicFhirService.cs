namespace EmrSimulator.Infrastructure.Providers.Epic;

public sealed class EpicFhirService(EpicLaunchOAuthService inner)
{
    public object Metadata() => new
    {
        resourceType = "CapabilityStatement",
        status = "active",
        rest = new[] { new { mode = "server" } }
    };

    public object Resource(string resource) => inner.FhirResource(resource);
}