namespace EmrSimulator.Tests.Integration.Cerner;

public sealed class VitalsLinkWorkflowTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Vitals_link_login_patient_and_device_routes_are_reachable()
    {
        var client = factory.CreateClient();

        var login = await client.PostAsync("/VitalsLink/login", null);
        var patient = await client.GetAsync("/VitalsLink/patients/CE-1001");
        var device = await client.PostAsync("/VitalsLink/devices/register", null);

        login.EnsureSuccessStatusCode();
        patient.EnsureSuccessStatusCode();
        device.EnsureSuccessStatusCode();
    }
}