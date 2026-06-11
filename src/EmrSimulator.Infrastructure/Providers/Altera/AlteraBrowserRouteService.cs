using EmrSimulator.Contracts.Unity;

namespace EmrSimulator.Infrastructure.Providers.Altera;

public sealed class AlteraBrowserRouteService
{
    public UnityBrowserRouteResponse Route(string routeName) => new(routeName, $"/{routeName}.aspx?synthetic=true", "Ready");
}