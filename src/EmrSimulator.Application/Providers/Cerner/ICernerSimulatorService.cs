using EmrSimulator.Contracts.Cerner;

namespace EmrSimulator.Application.Providers.Cerner;

public interface ICernerSimulatorService
{
    CernerAuthResponse Auth();
    CernerPatientResponse Patient(string? patientId = null);
    CernerDeviceResponse Device(string status);
    string Acknowledge(string message);
}