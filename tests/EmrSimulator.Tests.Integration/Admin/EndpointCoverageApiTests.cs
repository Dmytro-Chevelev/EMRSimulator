using System.Net.Http.Json;
using EmrSimulator.Contracts;

namespace EmrSimulator.Tests.Integration.Admin;

public sealed class EndpointCoverageApiTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Endpoint_coverage_api_returns_multiple_provider_families()
    {
        var client = factory.CreateClient();

        var contracts = await client.GetFromJsonAsync<List<EndpointContractDto>>("/api/v1/endpoint-contracts");

        Assert.NotNull(contracts);
        Assert.True(contracts!.Select(contract => contract.Provider).Distinct().Count() >= 3);
    }
}