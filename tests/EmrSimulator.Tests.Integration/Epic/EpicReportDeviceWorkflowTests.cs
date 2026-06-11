namespace EmrSimulator.Tests.Integration.Epic;

public sealed class EpicReportDeviceWorkflowTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Epic_report_and_device_routes_are_reachable()
    {
        var client = factory.CreateClient();

        var report = await client.PostAsync("/api/v1/Reports", null);
        var device = await client.PostAsync("/api/v1/DeviceWorkflow/start", null);

        report.EnsureSuccessStatusCode();
        device.EnsureSuccessStatusCode();
    }
}