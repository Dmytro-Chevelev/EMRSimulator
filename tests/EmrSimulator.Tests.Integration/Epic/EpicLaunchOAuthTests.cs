using System.Net.Http.Json;
using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Tests.Integration.Epic;

public sealed class EpicLaunchOAuthTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Epic_launch_and_token_routes_are_reachable()
    {
        var client = factory.CreateClient();

        var launch = await client.GetFromJsonAsync<EpicLaunchResponse>("/Midmark?launch=abc&iss=issuer");
        var token = await client.PostAsync("/oauth2/token", null);

        Assert.Equal("abc", launch?.LaunchToken);
        token.EnsureSuccessStatusCode();
    }
}