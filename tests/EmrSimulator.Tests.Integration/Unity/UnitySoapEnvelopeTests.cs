using System.Net.Http.Headers;

namespace EmrSimulator.Tests.Integration.Unity;

public sealed class UnitySoapEnvelopeTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Unity_service_returns_soap_response_envelope()
    {
        var client = factory.CreateClient();
        using var content = new StringContent("<s:Envelope><s:Body><Magic /></s:Body></s:Envelope>");
        content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
        content.Headers.Add("SOAPAction", "Magic");

        var response = await client.PostAsync("/Unity/UnityService.svc", content);
        var xml = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
        Assert.Contains("MagicResponse", xml, StringComparison.OrdinalIgnoreCase);
    }
}