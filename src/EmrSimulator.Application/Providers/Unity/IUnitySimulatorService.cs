using EmrSimulator.Contracts.Unity;

namespace EmrSimulator.Application.Providers.Unity;

public interface IUnitySimulatorService
{
    string HandleSoap(string? soapAction, string body);
    UnityBrowserRouteResponse BrowserRoute(string routeName);
}