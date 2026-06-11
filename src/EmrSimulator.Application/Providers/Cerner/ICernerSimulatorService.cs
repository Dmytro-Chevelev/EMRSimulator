namespace EmrSimulator.Application.Providers.Cerner;

public interface ICernerSimulatorService
{
    object Auth();
    object Patient(string? patientId = null);
    object Device(string status);
    string Acknowledge(string message);
}