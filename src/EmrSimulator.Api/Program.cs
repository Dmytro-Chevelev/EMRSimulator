using EmrSimulator.Api.Routes;
using EmrSimulator.Application;
using EmrSimulator.Contracts;
using EmrSimulator.Domain;
using EmrSimulator.Infrastructure;
using EmrSimulator.Infrastructure.Logging;
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

app.Use(async (context, next) =>
{
    if (!IsCompatibilityPath(context.Request.Path))
    {
        await next();
        return;
    }

    var path = context.Request.Path.Value ?? string.Empty;
    var provider = ResolveProvider(context.Request.Path);
    var catalog = context.RequestServices.GetRequiredService<IEndpointCatalogService>();
    var evidence = context.RequestServices.GetRequiredService<IVerificationEvidenceService>();
    var contract = catalog.FindByPathOrAction(path);
    var requiresAuth = contract?.AuthRequired ?? IsProtectedCompatibilityPath(context.Request.Path);

    if (requiresAuth
        && !context.RequestServices.GetRequiredService<ISyntheticAuthenticationService>()
            .Validate(provider, context.Request.Headers.Authorization.ToString()).Authorized)
    {
        const int statusCode = StatusCodes.Status401Unauthorized;
        context.RequestServices.GetRequiredService<ExternalEmrRequestLogger>()
            .Log(provider, path, context.Request.Method, statusCode, new { status = statusCode, error = "Unauthorized" });

        if (contract is not null)
        {
            evidence.Record(contract.Id, $"{context.Request.Method} {path}", statusCode.ToString(), false, "native-route-middleware");
        }

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new { error = "Synthetic credentials are required for this compatibility route." });
        return;
    }

    await next();

    context.RequestServices.GetRequiredService<ExternalEmrRequestLogger>()
        .Log(provider, path, context.Request.Method, context.Response.StatusCode, new { status = context.Response.StatusCode });

    if (contract is not null)
    {
        evidence.Record(contract.Id, $"{context.Request.Method} {path}", context.Response.StatusCode.ToString(), context.Response.StatusCode < 400, "native-route-middleware");
    }

    PersistGeneratedState(context);
});

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

app.MapGet("/api/v1/endpoint-contracts", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetEndpointContracts()))
    .WithName("GetEndpointContracts")
    .WithSummary("List external EMR endpoint coverage catalog")
    .WithDescription("Returns the simulator endpoint, operation, message, and data-source coverage catalog from the source EMR documents.")
    .Produces<IReadOnlyList<EndpointContractDto>>(StatusCodes.Status200OK)
    .WithOpenApi();

app.MapGet("/api/v1/endpoint-contracts/{endpointContractId:guid}/verification", (Guid endpointContractId, IEmrSimulatorFacade facade)
    => Results.Ok(facade.GetVerificationEvidence(endpointContractId)))
    .WithName("GetEndpointContractVerification")
    .WithSummary("List verification evidence for an endpoint contract")
    .WithDescription("Returns recorded verification evidence for a documented external EMR endpoint contract.")
    .Produces<IReadOnlyList<VerificationEvidenceDto>>(StatusCodes.Status200OK)
    .WithOpenApi();

app.MapPost("/api/v1/simulator/reset", (IEmrSimulatorFacade facade)
    => Results.Ok(facade.ResetSyntheticState()))
    .WithName("ResetSimulatorState")
    .WithSummary("Reset generated synthetic simulator state")
    .WithDescription("Clears generated reports, device registrations, documents, messages, request logs, and verification evidence while preserving endpoint definitions and default provider profiles.")
    .Produces<SimulatorResetResult>(StatusCodes.Status200OK)
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

    app.MapEpicCompatibilityEndpoints();
    app.MapCernerCompatibilityEndpoints();
    app.MapUnityCompatibilityEndpoints();

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

static bool IsCompatibilityPath(PathString path)
{
    var value = path.Value ?? string.Empty;
    return value.StartsWith("/Midmark", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/oauth2", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/metadata", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/FHIR", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/Pdf", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/VitalsLink", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/security", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/cas", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/gda", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/Unity", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/Altera", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/IQFrameworkWebService", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/Framework", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/Xbap", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/Allscripts", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/Reports", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/Devices", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/DeviceWorkflow", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/Authenticate", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/Register", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/ADTPatients", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/Physicians", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/HL7Messages", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/cerner", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/epic", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/unity", StringComparison.OrdinalIgnoreCase);
}

static bool IsProtectedCompatibilityPath(PathString path)
{
    var value = path.Value ?? string.Empty;
    return IsCompatibilityPath(path)
        && !value.StartsWith("/Midmark", StringComparison.OrdinalIgnoreCase)
        && !value.StartsWith("/metadata", StringComparison.OrdinalIgnoreCase)
        && !value.StartsWith("/Xbap", StringComparison.OrdinalIgnoreCase)
        && !value.StartsWith("/Allscripts", StringComparison.OrdinalIgnoreCase);
}

static string ResolveProvider(PathString path)
{
    var value = path.Value ?? string.Empty;
    if (value.StartsWith("/VitalsLink", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/security", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/cas", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/gda", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/ADTPatients", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/Physicians", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/HL7Messages", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/api/v1/cerner", StringComparison.OrdinalIgnoreCase))
    {
        return "Cerner";
    }

    if (value.StartsWith("/Unity", StringComparison.OrdinalIgnoreCase))
    {
        return "AthenaFlow";
    }

    if (value.StartsWith("/Altera", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/IQFrameworkWebService", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/Framework", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/Xbap", StringComparison.OrdinalIgnoreCase)
        || value.StartsWith("/Allscripts", StringComparison.OrdinalIgnoreCase))
    {
        return "Altera";
    }

    return "Epic";
}

static void PersistGeneratedState(HttpContext context)
{
    if (context.Response.StatusCode >= 400)
    {
        return;
    }

    var path = context.Request.Path.Value ?? string.Empty;
    var db = context.RequestServices.GetRequiredService<EmrSimulatorDbContext>();
    var now = DateTime.UtcNow;

    if (path.Contains("Report", StringComparison.OrdinalIgnoreCase)
        || path.Contains("chartdoc", StringComparison.OrdinalIgnoreCase))
    {
        db.SyntheticReportStates.Add(new SyntheticReportState
        {
            ScenarioId = Guid.Empty,
            EmrProfileId = Guid.Empty,
            ReportId = $"RPT-{now.Ticks}",
            ReportType = "Synthetic",
            DeviceId = "MM-DEVICE-001",
            Status = "Generated",
            ReportMetadataJson = "{}",
            ReportDataBase64 = string.Empty,
            PdfBase64 = string.Empty
        });
    }

    if (path.Contains("Device", StringComparison.OrdinalIgnoreCase) || path.Contains("devices", StringComparison.OrdinalIgnoreCase))
    {
        db.DeviceRegistrationStates.Add(new DeviceRegistrationState
        {
            ScenarioId = Guid.Empty,
            EmrProfileId = Guid.Empty,
            DeviceId = "MM-DEVICE-001",
            InstanceId = $"INSTANCE-{now.Ticks}",
            DisplayName = "Synthetic Device",
            DeviceType = "Vitals",
            Connected = true,
            ActiveWorkflowJson = "{}",
            CalibrationStateJson = "{}"
        });
    }

    if (path.Contains("Document", StringComparison.OrdinalIgnoreCase)
        || path.Contains("Unity", StringComparison.OrdinalIgnoreCase)
        || path.Contains("IQFramework", StringComparison.OrdinalIgnoreCase))
    {
        db.DocumentStates.Add(new DocumentState
        {
            ScenarioId = Guid.Empty,
            EmrProfileId = Guid.Empty,
            AccessionNumber = $"ACC-{now.Ticks}",
            DocumentType = "Synthetic",
            DocumentMetadataXml = "<Document />",
            DocumentImageBase64 = string.Empty,
            SourceOperation = path
        });
    }

    if (path.Contains("HL7", StringComparison.OrdinalIgnoreCase))
    {
        db.Hl7MessageStates.Add(new Hl7MessageState
        {
            ScenarioId = Guid.Empty,
            EmrProfileId = Guid.Empty,
            Direction = Hl7MessageDirection.Outbound,
            MessageType = "ORU",
            ControlId = $"CTRL-{now.Ticks}",
            PatientIdentifier = "EP-1001",
            RawMessage = "MSH|^~\\&|SIM|MIDMARK|CONNECTOR|LOCAL||ORU^R01|CTRL|P|2.5",
            AckMessage = "MSA|AA|CTRL",
            ValidationStatus = "Generated",
            SentAtUtc = now
        });
    }

    db.SaveChanges();
}

public partial class Program;
