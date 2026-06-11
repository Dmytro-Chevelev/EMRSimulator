using System.Net.Http.Json;

namespace EmrSimulator.Tests.Integration.Admin;

public sealed class VerificationEvidenceApiTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Verification_evidence_api_returns_empty_list_when_no_evidence_recorded()
    {
        var client = factory.CreateClient();
        var catalog = await client.GetFromJsonAsync<List<EmrSimulator.Contracts.EndpointContractDto>>("/api/v1/endpoint-contracts");

        var response = await client.GetAsync($"/api/v1/endpoint-contracts/{catalog![0].Id}/verification");
        var json = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
        Assert.Equal("[]", json);
    }
}