using System.Net.Http.Json;
using EmrSimulator.Contracts;

namespace EmrSimulator.Tests.Integration.Admin;

public sealed class SimulatorResetApiTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Reset_api_returns_generation_result()
    {
        var client = factory.CreateClient();

        var response = await client.PostAsync("/api/v1/simulator/reset", null);
        var result = await response.Content.ReadFromJsonAsync<SimulatorResetResult>();

        response.EnsureSuccessStatusCode();
        Assert.True(result?.ResetGeneration >= 1);
    }
}