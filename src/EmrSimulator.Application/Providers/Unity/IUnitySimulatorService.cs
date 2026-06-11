namespace EmrSimulator.Application.Providers.Unity;

public interface IUnitySimulatorService
{
    string HandleSoap(string? soapAction, string body);
    object BrowserRoute(string routeName);
}