using EmrSimulator.Application;
using EmrSimulator.Contracts;
using EmrSimulator.Infrastructure;

namespace EmrSimulator.Tests.Integration;

public class ProviderSwitchingTests
{
    [Fact]
    public void Active_provider_changes_when_switching()
    {
        var facade = new EmrSimulatorFacade(new InMemoryEmrSimulatorStore());

        var active = facade.SetActiveProvider(EmrProviderType.Cerner);

        Assert.Contains("Cerner", active.ActiveProvider, StringComparison.OrdinalIgnoreCase);
    }
}
