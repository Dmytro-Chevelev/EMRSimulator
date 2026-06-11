using EmrSimulator.Infrastructure.Providers.Altera;
using EmrSimulator.Infrastructure.Providers.Athena;
using EmrSimulator.Infrastructure.Providers.Unity;
using EmrSimulator.Infrastructure.Soap;

namespace EmrSimulator.Api.Routes;

public static class UnityEndpointMappings
{
    public static IEndpointRouteBuilder MapUnityCompatibilityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/Unity/UnityService.svc", async (HttpContext context, SoapEnvelopeService service) =>
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var response = service.HandleSoap(context.Request.Headers["SOAPAction"].ToString(), body);
            return Results.Text(response, "text/xml");
        });

        app.MapPost("/IQFrameworkWebService/IQConnectIF.asmx", async (HttpContext context, SoapEnvelopeService service) =>
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var response = service.HandleSoap(context.Request.Headers["SOAPAction"].ToString(), body);
            return Results.Text(response, "text/xml");
        });

        app.MapPost("/Unity/GetSecurityToken", (AthenaUnityService service) => Results.Ok(service.Token()));
        app.MapPost("/Unity/ReturnMagicJSON", (AthenaUnityService service) => Results.Ok(service.Magic("ReturnMagicJSON")));
        app.MapPost("/Unity/GetPatient", (AthenaUnityService service) => Results.Ok(service.Magic("GetPatient")));
        app.MapPost("/Altera/GetSecurityToken", (AlteraUnityService service) => Results.Ok(service.Token()));
        app.MapPost("/Altera/ReturnMagicJSON", (AlteraUnityService service) => Results.Ok(service.Magic("ReturnMagicJSON")));

        app.MapGet("/Xbap/{routeName}.aspx", (string routeName, AlteraBrowserRouteService service) => Results.Ok(service.Route(routeName)));
        app.MapGet("/Allscripts/{routeName}.aspx", (string routeName, AlteraBrowserRouteService service) => Results.Ok(service.Route(routeName)));
        app.MapGet("/XbapLauncher.aspx", (AlteraBrowserRouteService service) => Results.Ok(service.Route("XbapLauncher")));
        app.MapGet("/XbapTest.aspx", (AlteraBrowserRouteService service) => Results.Ok(service.Route("XbapTest")));
        app.MapGet("/XbapReview.aspx", (AlteraBrowserRouteService service) => Results.Ok(service.Route("XbapReview")));
        app.MapGet("/XbapCompare.aspx", (AlteraBrowserRouteService service) => Results.Ok(service.Route("XbapCompare")));
        app.MapGet("/XbapCalibrate.aspx", (AlteraBrowserRouteService service) => Results.Ok(service.Route("XbapCalibrate")));
        app.MapPost("/Framework/{operation}", (string operation, AlteraFrameworkService service) => Results.Ok(service.Operation(operation)));
        app.MapPost("/api/v1/unity/verification/{operation}", (string operation, UnityVerificationRecorder recorder) => Results.Ok(recorder.Record(operation)));

        return app;
    }
}