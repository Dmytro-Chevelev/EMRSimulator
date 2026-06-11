using EmrSimulator.Application;
using EmrSimulator.Contracts;
using EmrSimulator.Infrastructure;

namespace EmrSimulator.Tests.Contracts;

public class ProviderRoutesTests
{
    [Theory]
    [InlineData("epic")]
    [InlineData("cerner")]
    [InlineData("altera")]
    [InlineData("athena-flow")]
    [InlineData("athena-server")]
    public void Provider_search_routes_return_payload_in_happy_path(string provider)
    {
        var facade = new EmrSimulatorFacade(new InMemoryEmrSimulatorStore());
        facade.SetActiveScenario(ScenarioType.HappyPath);

        var result = facade.ExecuteProviderRoute(provider, $"/api/v1/emr/{provider}/patients/search", "GET");

        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Payload);
    }
}
