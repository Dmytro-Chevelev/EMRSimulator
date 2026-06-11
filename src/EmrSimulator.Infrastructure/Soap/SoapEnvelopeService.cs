using EmrSimulator.Application.Providers.Unity;
using EmrSimulator.Contracts.Unity;

namespace EmrSimulator.Infrastructure.Soap;

public sealed class SoapEnvelopeService : IUnitySimulatorService
{
    public string HandleSoap(string? soapAction, string body)
    {
        var operation = ResolveOperation(soapAction, body);
        return UnitySampleBuilder.SoapEnvelope(operation);
    }

    public UnityBrowserRouteResponse BrowserRoute(string routeName) => new(routeName, $"/{routeName}.aspx?synthetic=true", "Ready");

    private static string ResolveOperation(string? soapAction, string body)
    {
        if (!string.IsNullOrWhiteSpace(soapAction))
        {
            return soapAction.Split('/').Last().Trim('"');
        }

        if (body.Contains("GetSecurityToken", StringComparison.OrdinalIgnoreCase))
        {
            return "GetSecurityToken";
        }

        if (body.Contains("ReturnMagicJSON", StringComparison.OrdinalIgnoreCase))
        {
            return "ReturnMagicJSON";
        }

        return "Magic";
    }
}