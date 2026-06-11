namespace EmrSimulator.Application.Providers.Epic;

public interface IEpicSimulatorService
{
    object Launch(string? launchToken, string? issuer);
    object Token();
    object FhirResource(string resource);
    object Report(string? reportId = null);
    object Device(string status);
}