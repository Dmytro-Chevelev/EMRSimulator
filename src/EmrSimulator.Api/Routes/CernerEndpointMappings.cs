using EmrSimulator.Contracts.Cerner;
using EmrSimulator.Infrastructure.Providers.Cerner;

namespace EmrSimulator.Api.Routes;

public static class CernerEndpointMappings
{
    public static IEndpointRouteBuilder MapCernerCompatibilityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/VitalsLink/login", (VitalsLinkAuthService service) => Results.Ok(service.Auth()));
        app.MapPost("/security/auth/login", (VitalsLinkAuthService service) => Results.Ok(service.Auth()));
        app.MapGet("/VitalsLink/barcodes", (VitalsLinkClinicalService service) => Results.Ok(service.BarcodeFormats()));
        app.MapGet("/cas/api/v1/barcode/formats", (VitalsLinkClinicalService service) => Results.Ok(service.BarcodeFormats()));
        app.MapGet("/VitalsLink/personnel/{barcode}", (string barcode, VitalsLinkClinicalService service) => Results.Ok(service.Personnel(barcode)));
        app.MapGet("/cas/api/v1/barcode/organizations/{organizationId}/barcodes/{barcode}/personnel", (string barcode, VitalsLinkClinicalService service) => Results.Ok(service.Personnel(barcode)));
        app.MapGet("/VitalsLink/locations", (VitalsLinkClinicalService service) => Results.Ok(service.Locations()));
        app.MapGet("/cas/api/v1/locations/getLocations", (VitalsLinkClinicalService service) => Results.Ok(service.Locations()));
        app.MapGet("/cas/api/v1/encounters", (VitalsLinkClinicalService service) => Results.Ok(service.Encounter()));
        app.MapGet("/VitalsLink/encounters/{encounterId}", (string encounterId, VitalsLinkClinicalService service) => Results.Ok(service.Encounter(encounterId)));
        app.MapGet("/cas/api/v1/encounters/{encounterId}", (string encounterId, VitalsLinkClinicalService service) => Results.Ok(service.Encounter(encounterId)));
        app.MapGet("/VitalsLink/patients/{patientId}", (string patientId, VitalsLinkClinicalService service) => Results.Ok(service.Patient(patientId)));
        app.MapGet("/cas/api/v1/patients", (string? _id, VitalsLinkClinicalService service) => Results.Ok(service.Patient(_id)));
        app.MapPost("/VitalsLink/devices/register", (VitalsLinkDeviceService service) => Results.Ok(service.Register()));
        app.MapPost("/gda/api/devices", (VitalsLinkDeviceService service) => Results.Ok(service.Register()));
        app.MapPost("/VitalsLink/devices/heartbeat", (VitalsLinkDeviceService service) => Results.Ok(service.Heartbeat()));
        app.MapPost("/gda/api/devices/heartbeat", (VitalsLinkDeviceService service) => Results.Ok(service.Heartbeat()));
        app.MapPost("/VitalsLink/vitals", (VitalsLinkDeviceService service) => Results.Ok(service.PostVitals()));
        app.MapPost("/cas/api/v1/chartdoc/discrete", (VitalsLinkDeviceService service) => Results.Ok(service.PostVitals()));
        app.MapDelete("/VitalsLink/devices/{deviceId}/{instanceId}", (string deviceId, string instanceId, VitalsLinkDeviceService service) => Results.Ok(service.Remove(deviceId, instanceId)));
        app.MapDelete("/gda/api/devices/{deviceId}/{instanceId}", (string deviceId, string instanceId, VitalsLinkDeviceService service) => Results.Ok(service.Remove(deviceId, instanceId)));

        app.MapGet("/api/v1/cerner/patients", (CernerMidmarkService service) => Results.Ok(service.SearchPatients()));
        app.MapGet("/api/v1/cerner/patients/{patientId}", (string patientId, CernerMidmarkService service) => Results.Ok(service.Patient(patientId)));
        app.MapGet("/api/v1/cerner/physicians", (CernerMidmarkService service) => Results.Ok(service.Physicians()));
        app.MapPost("/api/v1/cerner/hl7/submissions", (CernerMidmarkService service) => Results.Ok(service.Hl7Submitted()));
        app.MapGet("/api/v1/cerner/hl7/submissions/{messageId}", (string messageId, CernerMidmarkService service) => Results.Ok(service.Hl7Submitted(messageId)));
        app.MapPost("/api/v1/ADTPatients/PatientSearchRequest", (CernerMidmarkService service) => Results.Ok(service.SearchPatients()));
        app.MapGet("/api/v1/ADTPatients/{patientId}", (string patientId, CernerMidmarkService service) => Results.Ok(service.Patient(patientId)));
        app.MapPut("/api/v1/ADTPatients/UpdateLastAccessTime", () => Results.Ok(new CernerLastAccessUpdateResponse("Updated")));
        app.MapGet("/api/v1/Physicians", (CernerMidmarkService service) => Results.Ok(service.Physicians()));
        app.MapPost("/api/v1/HL7Messages", (CernerMidmarkService service) => Results.Ok(service.Hl7Submitted()));
        app.MapPost("/api/v1/HL7Messages/pendingtest/{messageId}", (string messageId, CernerMidmarkService service) => Results.Ok(service.Hl7Submitted(messageId)));
        app.MapPost("/api/v1/cerner/verification/{route}", (string route, CernerVerificationRecorder recorder) => Results.Ok(recorder.Record(route)));

        return app;
    }
}