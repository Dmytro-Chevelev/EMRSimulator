using EmrSimulator.Application;
using EmrSimulator.Contracts;
using EmrSimulator.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEmrSimulatorInfrastructure();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/v1/providers", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetProviders()))
    .WithName("GetProviders")
    .WithOpenApi();

app.MapGet("/api/v1/providers/active", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetActiveProvider()))
    .WithName("GetActiveProvider")
    .WithOpenApi();

app.MapPut("/api/v1/providers/active/{provider}", (string provider, IEmrSimulatorFacade facade) =>
{
    if (!Enum.TryParse<EmrProviderType>(provider, true, out var parsedProvider))
    {
        return Results.BadRequest(new { error = "Invalid provider" });
    }

    return Results.Ok(facade.SetActiveProvider(parsedProvider));
})
.WithName("SetActiveProvider")
.WithOpenApi();

app.MapGet("/api/v1/scenarios", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetScenarios()))
    .WithName("GetScenarios")
    .WithOpenApi();

app.MapPut("/api/v1/scenarios/active/{scenario}", (string scenario, IEmrSimulatorFacade facade) =>
{
    if (!Enum.TryParse<ScenarioType>(scenario, true, out var parsedScenario))
    {
        return Results.BadRequest(new { error = "Invalid scenario" });
    }

    return Results.Ok(facade.SetActiveScenario(parsedScenario));
})
.WithName("SetActiveScenario")
.WithOpenApi();

app.MapGet("/api/v1/patients", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetPatients()))
    .WithName("GetPatients")
    .WithOpenApi();

app.MapGet("/api/v1/patients/{patientId:guid}", (Guid patientId, IEmrSimulatorFacade facade)
    => facade.GetPatient(patientId) is { } patient ? Results.Ok(patient) : Results.NotFound())
    .WithName("GetPatient")
    .WithOpenApi();

app.MapGet("/api/v1/appointments", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetAppointments()))
    .WithName("GetAppointments")
    .WithOpenApi();

app.MapGet("/api/v1/orders", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetOrders()))
    .WithName("GetOrders")
    .WithOpenApi();

app.MapGet("/api/v1/results", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetResults()))
    .WithName("GetResults")
    .WithOpenApi();

app.MapGet("/api/v1/request-logs", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetRequestLogs()))
    .WithName("GetRequestLogs")
    .WithOpenApi();

app.MapPost("/api/v1/import/patients", async (HttpContext context, IEmrSimulatorFacade facade) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var content = await reader.ReadToEndAsync();
    var report = facade.ImportPatients("csv", content);
    return Results.Ok(report);
})
.WithName("ImportPatients")
.WithOpenApi();

app.MapPost("/api/v1/emr/{provider}/auth/token", (string provider, IEmrSimulatorFacade facade)
    => ToRouteResult(facade.ExecuteProviderRoute(provider, $"/api/v1/emr/{provider}/auth/token", "POST")))
    .WithName("ProviderAuthToken")
    .WithOpenApi();

app.MapGet("/api/v1/emr/{provider}/patients/search", (string provider, IEmrSimulatorFacade facade)
    => ToRouteResult(facade.ExecuteProviderRoute(provider, $"/api/v1/emr/{provider}/patients/search", "GET")))
    .WithName("ProviderPatientSearch")
    .WithOpenApi();

app.MapGet("/api/v1/emr/{provider}/patients/{patientId}", (string provider, string patientId, IEmrSimulatorFacade facade)
    => ToRouteResult(facade.ExecuteProviderRoute(provider, $"/api/v1/emr/{provider}/patients/{patientId}", "GET", patientId)))
    .WithName("ProviderPatientDetails")
    .WithOpenApi();

app.Run();

static IResult ToRouteResult(ProviderRouteResult result)
{
    if (result.StatusCode >= 400)
    {
        return Results.Json(new { provider = result.Provider, route = result.Route, error = result.Error }, statusCode: result.StatusCode);
    }

    return Results.Json(result.Payload, statusCode: result.StatusCode);
}

public partial class Program;
