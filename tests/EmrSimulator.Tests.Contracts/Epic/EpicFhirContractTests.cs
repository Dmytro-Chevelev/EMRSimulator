using EmrSimulator.Infrastructure.Providers.Epic;

namespace EmrSimulator.Tests.Contracts.Epic;

public sealed class EpicFhirContractTests
{
    [Fact]
    public void Fhir_metadata_returns_capability_statement_shape()
    {
        var service = new EpicFhirService(new EpicLaunchOAuthService());

        var metadata = service.Metadata();

        Assert.Contains("CapabilityStatement", metadata.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}