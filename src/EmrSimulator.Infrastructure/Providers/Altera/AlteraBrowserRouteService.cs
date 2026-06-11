namespace EmrSimulator.Infrastructure.Providers.Altera;

public sealed class AlteraBrowserRouteService
{
    public object Route(string routeName) => new { route = routeName, url = $"/{routeName}.aspx?synthetic=true", status = "Ready" };
}