using EmrSimulator.Application;
using EmrSimulator.Contracts;
using EmrSimulator.Infrastructure;
using EmrSimulator.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddEmrSimulatorInfrastructure(connectionString);

var app = builder.Build();

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

// Ensure the SQLite schema exists on first run
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EmrSimulatorDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/api/v1/providers", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetProviders()))
    .WithName("GetProviders")
    .WithSummary("List configured EMR providers")
    .WithDescription("Returns all supported EMR providers and highlights which provider is currently active.")
    .Produces<IReadOnlyList<ProviderSelectionDto>>(StatusCodes.Status200OK)
    .WithOpenApi();

app.MapGet("/api/v1/providers/active", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetActiveProvider()))
    .WithName("GetActiveProvider")
    .WithSummary("Get active EMR provider")
    .WithDescription("Returns the provider currently selected for simulated provider routes.")
    .Produces<ProviderSelectionDto>(StatusCodes.Status200OK)
    .WithOpenApi();

app.MapPut("/api/v1/providers/active/{provider}", (string provider, IEmrSimulatorFacade facade) =>
{
    if (!Enum.TryParse<EmrProviderType>(provider, true, out var parsedProvider))
    {
        return Results.ValidationProblem(CreateValidationProblem("provider", "Invalid provider"));
    }

    return Results.Ok(facade.SetActiveProvider(parsedProvider));
})
.WithName("SetActiveProvider")
.WithSummary("Set active EMR provider")
.WithDescription("Updates the active provider used by provider-specific simulation endpoints.")
.Produces<ProviderSelectionDto>(StatusCodes.Status200OK)
.ProducesValidationProblem(StatusCodes.Status400BadRequest)
.WithOpenApi();

app.MapGet("/api/v1/scenarios", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetScenarios()))
    .WithName("GetScenarios")
    .WithSummary("List available simulation scenarios")
    .WithDescription("Returns all supported runtime simulation scenarios.")
    .Produces<IReadOnlyList<ScenarioDto>>(StatusCodes.Status200OK)
    .WithOpenApi();

app.MapPut("/api/v1/scenarios/active/{scenario}", (string scenario, IEmrSimulatorFacade facade) =>
{
    if (!Enum.TryParse<ScenarioType>(scenario, true, out var parsedScenario))
    {
        return Results.ValidationProblem(CreateValidationProblem("scenario", "Invalid scenario"));
    }

    return Results.Ok(facade.SetActiveScenario(parsedScenario));
})
.WithName("SetActiveScenario")
.WithSummary("Set active simulation scenario")
.WithDescription("Sets the global scenario that influences downstream provider route responses.")
.Produces<ScenarioDto>(StatusCodes.Status200OK)
.ProducesValidationProblem(StatusCodes.Status400BadRequest)
.WithOpenApi();

app.MapGet("/api/v1/patients", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetPatients()))
    .WithName("GetPatients")
    .WithSummary("List simulated patients")
    .WithDescription("Returns all simulated patient records currently loaded in the simulator store.")
    .Produces<IReadOnlyList<PatientDto>>(StatusCodes.Status200OK)
    .WithOpenApi();

app.MapGet("/api/v1/patients/{patientId:guid}", (Guid patientId, IEmrSimulatorFacade facade)
    => facade.GetPatient(patientId) is { } patient ? Results.Ok(patient) : Results.NotFound())
    .WithName("GetPatient")
    .WithSummary("Get simulated patient by ID")
    .WithDescription("Returns a single patient by internal simulator identifier.")
    .Produces<PatientDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

app.MapGet("/api/v1/appointments", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetAppointments()))
    .WithName("GetAppointments")
    .WithSummary("List simulated appointments")
    .WithDescription("Returns all simulated appointment records.")
    .Produces<IReadOnlyList<AppointmentDto>>(StatusCodes.Status200OK)
    .WithOpenApi();

app.MapGet("/api/v1/orders", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetOrders()))
    .WithName("GetOrders")
    .WithSummary("List simulated orders")
    .WithDescription("Returns all simulated orders generated for patients.")
    .Produces<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK)
    .WithOpenApi();

app.MapGet("/api/v1/results", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetResults()))
    .WithName("GetResults")
    .WithSummary("List simulated results")
    .WithDescription("Returns all simulated clinical results linked to orders and patients.")
    .Produces<IReadOnlyList<ResultDto>>(StatusCodes.Status200OK)
    .WithOpenApi();

app.MapGet("/api/v1/request-logs", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetRequestLogs()))
    .WithName("GetRequestLogs")
    .WithSummary("List simulator request logs")
    .WithDescription("Returns recorded route executions including latency, response code, and scenario context.")
    .Produces<IReadOnlyList<RequestLogDto>>(StatusCodes.Status200OK)
    .WithOpenApi();

app.MapPost("/api/v1/import/patients", async (HttpContext context, IEmrSimulatorFacade facade) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var content = await reader.ReadToEndAsync();
    var report = facade.ImportPatients("csv", content);
    return Results.Ok(report);
})
.WithName("ImportPatients")
.WithSummary("Import patients from CSV payload")
.WithDescription("Imports CSV rows from the request body and returns accepted/rejected row details.")
.Produces<ImportReport>(StatusCodes.Status200OK)
.WithOpenApi();

app.MapPost("/api/v1/emr/{provider}/auth/token", (string provider, IEmrSimulatorFacade facade)
    => ToRouteResult(facade.ExecuteProviderRoute(provider, $"/api/v1/emr/{provider}/auth/token", "POST")))
    .WithName("ProviderAuthToken")
    .WithSummary("Simulate provider auth token route")
    .WithDescription("Executes the provider auth token route using the active scenario behavior.")
    .Produces(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .WithOpenApi();

app.MapGet("/api/v1/emr/{provider}/patients/search", (string provider, IEmrSimulatorFacade facade)
    => ToRouteResult(facade.ExecuteProviderRoute(provider, $"/api/v1/emr/{provider}/patients/search", "GET")))
    .WithName("ProviderPatientSearch")
    .WithSummary("Simulate provider patient search route")
    .WithDescription("Executes provider-specific patient search behavior for the active scenario.")
    .Produces(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .WithOpenApi();

app.MapGet("/api/v1/emr/{provider}/patients/{patientId}", (string provider, string patientId, IEmrSimulatorFacade facade)
    => ToRouteResult(facade.ExecuteProviderRoute(provider, $"/api/v1/emr/{provider}/patients/{patientId}", "GET", patientId)))
    .WithName("ProviderPatientDetails")
    .WithSummary("Simulate provider patient detail route")
    .WithDescription("Executes provider-specific patient detail behavior for a specific external patient identifier.")
    .Produces(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status500InternalServerError)
    .WithOpenApi();

app.Run();

static IResult ToRouteResult(ProviderRouteResult result)
{
    if (result.StatusCode >= 400)
    {
        return Results.Problem(
            statusCode: result.StatusCode,
            title: result.Error ?? "Provider route failed",
            extensions: new Dictionary<string, object?>
            {
                ["provider"] = result.Provider,
                ["route"] = result.Route
            });
    }

    return Results.Json(result.Payload, statusCode: result.StatusCode);
}

static IDictionary<string, string[]> CreateValidationProblem(string field, string message)
    => new Dictionary<string, string[]>
    {
        [field] = [message]
    };

public partial class Program;
