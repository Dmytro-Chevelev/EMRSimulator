using EmrSimulator.Application;
using EmrSimulator.Application.Repositories;
using EmrSimulator.Contracts;
using EmrSimulator.Domain;

namespace EmrSimulator.Infrastructure.ExternalEmr;

public sealed class VerificationEvidenceService(
    IEndpointContractRepository endpointContractRepository,
    IVerificationEvidenceRepository evidenceRepository) : IVerificationEvidenceService
{
    public IReadOnlyList<VerificationEvidenceDto> GetEvidence(Guid? endpointContractId = null)
        => evidenceRepository.GetAll(endpointContractId).Select(ToDto).ToList();

    public VerificationEvidenceDto Record(Guid endpointContractId, string verificationName, string actualStatus, bool passed, string toolOrTestName)
    {
        var contract = endpointContractRepository.GetById(endpointContractId)
            ?? throw new InvalidOperationException($"Endpoint contract {endpointContractId} was not found.");

        var evidence = evidenceRepository.Add(new VerificationEvidence
        {
            EndpointContractId = endpointContractId,
            EmrProfileId = Guid.Empty,
            VerificationName = verificationName,
            RequestSampleReference = contract.ContractKey,
            ExpectedOutcome = "Contract-valid synthetic response",
            ActualStatus = actualStatus,
            ActualResponseSummary = actualStatus,
            Passed = passed,
            ToolOrTestName = toolOrTestName
        });

        return ToDto(evidence);
    }

    private static VerificationEvidenceDto ToDto(VerificationEvidence evidence)
        => new(
            evidence.Id,
            evidence.EndpointContractId,
            evidence.EmrProfileId,
            evidence.ScenarioId,
            evidence.VerificationName,
            evidence.ExpectedOutcome,
            evidence.ActualStatus,
            evidence.ActualResponseSummary,
            evidence.Passed,
            evidence.FailureReason,
            evidence.VerifiedAtUtc,
            evidence.ToolOrTestName);
}