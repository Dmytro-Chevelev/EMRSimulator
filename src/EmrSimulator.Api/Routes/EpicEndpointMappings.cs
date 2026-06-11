using EmrSimulator.Infrastructure.Providers.Epic;

namespace EmrSimulator.Api.Routes;

public static class EpicEndpointMappings
{
    public static IEndpointRouteBuilder MapEpicCompatibilityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/Midmark", (string? launch, string? iss, EpicLaunchOAuthService service)
            => Results.Ok(service.Launch(launch, iss)));

        app.MapGet("/Midmark/Redirect", (EpicLaunchOAuthService service)
            => Results.Ok(service.Launch("synthetic-launch", "http://localhost:5288/FHIR/R4")));

        app.MapGet("/Midmark/Close", () => Results.Ok(new { status = "Closed" }));

        app.MapPost("/oauth2/token", (EpicLaunchOAuthService service) => Results.Ok(service.Token()));
        app.MapGet("/metadata", (EpicFhirService service) => Results.Ok(service.Metadata()));
        app.MapGet("/FHIR/R4/{**resource}", (string resource, EpicFhirService service) => Results.Ok(service.Resource(resource)));

        app.MapGet("/Pdf/convert", (string? documentId, EpicPdfService service) => Results.Ok(service.Convert(documentId)));
        app.MapPost("/Pdf/convert", (string? documentId, EpicPdfService service) => Results.Ok(service.Convert(documentId)));

        app.MapGet("/api/v1/Reports", (string patientId, EpicReportsService service) => Results.Ok(service.List(patientId)));
        app.MapPost("/api/v1/Reports", (EpicReportsService service) => Results.Ok(service.Save()));
        app.MapGet("/api/v1/Reports/{reportId}", (string reportId, EpicReportsService service) => Results.Ok(service.Get(reportId)));
        app.MapGet("/api/v1/Reports/patientId/{patientId}", (string patientId, EpicReportsService service) => Results.Ok(service.List(patientId)));
        app.MapGet("/api/v1/Reports/ReportType/{reportType}", (string reportType, EpicReportsService service) => Results.Ok(service.List(reportType)));
        app.MapGet("/api/v1/Reports/deviceId/{deviceId}", (string deviceId, EpicReportsService service) => Results.Ok(service.List(deviceId)));
        app.MapGet("/api/v1/Reports/reportId/{reportId}", (string reportId, EpicReportsService service) => Results.Ok(service.Get(reportId)));
        app.MapPost("/api/v1/Reports/SaveReport", (EpicReportsService service) => Results.Ok(service.Save()));
        app.MapGet("/api/v1/Reports/GetDataFile", () => Results.Ok(new { fileId = "DATA-1001", status = "Available" }));
        app.MapPost("/api/v1/Reports/ReviewReport/{reportId}", (string reportId, EpicReportsService service) => Results.Ok(service.Get(reportId)));
        app.MapPost("/api/v1/Reports/CompareReports", () => Results.Ok(new { comparisonId = "CMP-1001", status = "Compared" }));
        app.MapPost("/api/v1/Reports/Convert/{reportType}/{reportId}", (string reportType, string reportId, EpicReportsService service) => Results.Ok(service.Get(reportId)));

        app.MapPost("/api/v1/DeviceWorkflow/start", (EpicDeviceWorkflowService service) => Results.Ok(service.Start()));
        app.MapPost("/api/v1/DeviceWorkflow/abort", (EpicDeviceWorkflowService service) => Results.Ok(service.Abort()));
        app.MapPost("/api/v1/DeviceWorkflow/launcher", (EpicDeviceWorkflowService service) => Results.Ok(service.RegisterLauncher()));
        app.MapPost("/api/v1/Devices/StartTest", (EpicDeviceWorkflowService service) => Results.Ok(service.Start()));
        app.MapPost("/api/v1/Devices/Abort", (EpicDeviceWorkflowService service) => Results.Ok(service.Abort()));
        app.MapPost("/api/v1/Authenticate/Auth", () => Results.Ok(new { status = "Authenticated", scheme = "Synthetic" }));
        app.MapPost("/api/v1/Register/Launcher", (EpicDeviceWorkflowService service) => Results.Ok(service.RegisterLauncher()));

        app.MapPost("/api/v1/epic/verification/{route}", (string route, EpicVerificationRecorder recorder) => Results.Ok(recorder.Record(route)));

        return app;
    }
}