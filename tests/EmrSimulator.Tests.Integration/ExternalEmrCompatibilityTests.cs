using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EmrSimulator.Contracts;
using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Tests.Integration;

public sealed class ExternalEmrCompatibilityTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Endpoint_catalog_is_available_under_versioned_admin_api()
    {
        var client = factory.CreateClient();

        var contracts = await client.GetFromJsonAsync<List<EndpointContractDto>>("/api/v1/endpoint-contracts");

        Assert.NotNull(contracts);
        Assert.Contains(contracts!, contract => contract.ContractKey == "epic-launch");
        Assert.Contains(contracts!, contract => contract.Provider == EmrProviderType.Cerner);
    }

    [Fact]
    public async Task Simulator_reset_clears_generated_state_and_returns_generation()
    {
        var client = factory.CreateClient();

        var result = await client.PostAsync("/api/v1/simulator/reset", content: null);

        result.EnsureSuccessStatusCode();
        var reset = await result.Content.ReadFromJsonAsync<SimulatorResetResult>();

        Assert.NotNull(reset);
        Assert.True(reset!.ResetGeneration >= 1);
    }

    [Fact]
    public async Task Epic_native_launch_and_fhir_routes_return_synthetic_payloads()
    {
        var client = factory.CreateClient();

        var launch = await client.GetFromJsonAsync<EpicLaunchResponse>("/Midmark?launch=test-launch&iss=test-issuer");
        var patient = await client.GetAsync("/FHIR/R4/Patient/EP-1001");

        Assert.NotNull(launch);
        Assert.Equal("test-launch", launch!.LaunchToken);
        Assert.Equal(HttpStatusCode.OK, patient.StatusCode);
    }

    [Fact]
    public async Task Cerner_patient_list_returns_seeded_database_patients()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/cerner/patients");
        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);

        response.EnsureSuccessStatusCode();
        Assert.Equal(15, document.RootElement.GetArrayLength());
        Assert.Contains("ADT-1001", json, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("MRN-1015", json, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Unity_soap_endpoint_preserves_xml_framing()
    {
        var client = factory.CreateClient();
        using var content = new StringContent("<s:Envelope><s:Body><GetSecurityToken /></s:Body></s:Envelope>");
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");
        content.Headers.Add("SOAPAction", "GetSecurityToken");

        var response = await client.PostAsync("/Unity/UnityService.svc", content);
        var xml = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
        Assert.Contains("Envelope", xml, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("GetSecurityTokenResponse", xml, StringComparison.OrdinalIgnoreCase);
    }
}