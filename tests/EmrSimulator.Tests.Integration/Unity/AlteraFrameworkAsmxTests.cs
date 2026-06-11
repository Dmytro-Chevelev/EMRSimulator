using System.Net.Http.Headers;

namespace EmrSimulator.Tests.Integration.Unity;

public sealed class AlteraFrameworkAsmxTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Altera_framework_asmx_route_returns_xml_envelope()
    {
        var client = factory.CreateClient();
        using var content = new StringContent("<s:Envelope><s:Body><GetFile /></s:Body></s:Envelope>");
        content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
        content.Headers.Add("SOAPAction", "GetFile");

        var response = await client.PostAsync("/IQFrameworkWebService/IQConnectIF.asmx", content);
        var xml = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
        Assert.Contains("GetFileResponse", xml, StringComparison.OrdinalIgnoreCase);
    }
}