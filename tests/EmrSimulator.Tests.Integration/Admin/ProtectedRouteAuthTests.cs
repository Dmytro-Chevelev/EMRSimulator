namespace EmrSimulator.Tests.Integration.Admin;

public sealed class ProtectedRouteAuthTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Protected_native_route_rejects_missing_synthetic_credentials()
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = null;

        var response = await client.GetAsync("/FHIR/R4/Patient/EP-1001");

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}