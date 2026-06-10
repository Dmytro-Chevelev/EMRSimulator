using EmrSimulator.Application;
using EmrSimulator.Contracts;
using EmrSimulator.Infrastructure;

namespace EmrSimulator.Tests.Unit;

public class ScenarioEngineTests
{
    [Fact]
    public void Scenario_selection_is_deterministic_for_same_request()
    {
        var facade = new EmrSimulatorFacade(new InMemoryEmrSimulatorStore());

        facade.SetActiveScenario(ScenarioType.PatientNotFound);

        var first = facade.ExecuteProviderRoute("epic", "/api/v1/emr/epic/patients/search", "GET");
        var second = facade.ExecuteProviderRoute("epic", "/api/v1/emr/epic/patients/search", "GET");

        Assert.Equal(first.StatusCode, second.StatusCode);
        Assert.Equal(first.Error, second.Error);
    }

    [Fact]
    public void Happy_path_returns_success()
    {
        var facade = new EmrSimulatorFacade(new InMemoryEmrSimulatorStore());

        facade.SetActiveScenario(ScenarioType.HappyPath);

        var result = facade.ExecuteProviderRoute("cerner", "/api/v1/emr/cerner/patients/search", "GET");

        Assert.Equal(200, result.StatusCode);
        Assert.Null(result.Error);
    }
}
