using EmrSimulator.Contracts;

namespace EmrSimulator.Application;

public interface IEndpointCatalogService
{
    IReadOnlyList<EndpointContractDto> GetEndpointContracts();
    EndpointContractDto? FindByPathOrAction(string pathOrAction);
}

public interface IContractValidationService
{
    ContractValidationResult Validate(string contractFamily, string? payload);
}

public interface ISyntheticAuthenticationService
{
    SyntheticAuthResult Validate(string provider, string? authorizationHeader, string? token = null);
}

public interface IVerificationEvidenceService
{
    IReadOnlyList<VerificationEvidenceDto> GetEvidence(Guid? endpointContractId = null);
    VerificationEvidenceDto Record(Guid endpointContractId, string verificationName, string actualStatus, bool passed, string toolOrTestName);
}

public interface ISyntheticScenarioStateService
{
    SimulatorResetResult Reset();
    int ResetGeneration { get; }
}