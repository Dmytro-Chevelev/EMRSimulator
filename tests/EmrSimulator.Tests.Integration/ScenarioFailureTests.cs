using EmrSimulator.Application;
using EmrSimulator.Contracts;
using EmrSimulator.Infrastructure;

namespace EmrSimulator.Tests.Integration;

public class ScenarioFailureTests
{
    [Fact]
    public void Timeout_scenario_returns_gateway_timeout()
    {
        var facade = new EmrSimulatorFacade(new InMemoryEmrSimulatorStore());
        facade.SetActiveScenario(ScenarioType.Timeout);

        var result = facade.ExecuteProviderRoute("epic", "/api/v1/emr/epic/patients/search", "GET");

        Assert.Equal(504, result.StatusCode);
    }
}
