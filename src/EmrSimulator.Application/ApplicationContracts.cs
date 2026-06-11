using EmrSimulator.Contracts;

namespace EmrSimulator.Application;

public interface IEmrSimulatorFacade
{
    IReadOnlyList<ProviderSelectionDto> GetProviders();
    ProviderSelectionDto GetActiveProvider();
    ProviderSelectionDto SetActiveProvider(EmrProviderType provider);

    IReadOnlyList<PatientDto> GetPatients();
    PatientDto? GetPatient(Guid id);
    IReadOnlyList<AppointmentDto> GetAppointments();
    IReadOnlyList<OrderDto> GetOrders();
    IReadOnlyList<ResultDto> GetResults();

    IReadOnlyList<ScenarioDto> GetScenarios();
    ScenarioDto SetActiveScenario(ScenarioType scenarioType);

    ProviderRouteResult ExecuteProviderRoute(string provider, string route, string method, string? patientId = null, string? requestBody = null);
    ImportReport ImportPatients(string sourceFormat, string content);
    IReadOnlyList<RequestLogDto> GetRequestLogs();
    IReadOnlyList<EndpointContractDto> GetEndpointContracts();
    IReadOnlyList<VerificationEvidenceDto> GetVerificationEvidence(Guid? endpointContractId = null);
    SimulatorResetResult ResetSyntheticState();
}
