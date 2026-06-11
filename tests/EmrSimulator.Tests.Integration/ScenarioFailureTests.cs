using EmrSimulator.Application;
using EmrSimulator.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace EmrSimulator.Tests.Integration;

public class ScenarioFailureTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public void Timeout_scenario_returns_gateway_timeout()
    {
        using var scope = factory.Services.CreateScope();
        var facade = scope.ServiceProvider.GetRequiredService<IEmrSimulatorFacade>();
        facade.SetActiveScenario(ScenarioType.Timeout);

        var result = facade.ExecuteProviderRoute("epic", "/api/v1/emr/epic/patients/search", "GET");

        Assert.Equal(504, result.StatusCode);
    }
}
