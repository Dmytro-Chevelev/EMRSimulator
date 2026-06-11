using EmrSimulator.Application;
using EmrSimulator.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace EmrSimulator.Tests.Integration;

public class ProviderSwitchingTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public void Active_provider_changes_when_switching()
    {
        using var scope = factory.Services.CreateScope();
        var facade = scope.ServiceProvider.GetRequiredService<IEmrSimulatorFacade>();

        var active = facade.SetActiveProvider(EmrProviderType.Cerner);

        Assert.Contains("Cerner", active.ActiveProvider, StringComparison.OrdinalIgnoreCase);
    }
}
