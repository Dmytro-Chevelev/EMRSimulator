using EmrSimulator.Domain;

namespace EmrSimulator.Application.Repositories;

public interface IEndpointContractRepository
{
    IReadOnlyList<EndpointContract> GetAll();
    EndpointContract? GetById(Guid id);
    EndpointContract? FindByPathOrAction(string pathOrAction);
    void UpsertRange(IEnumerable<EndpointContract> contracts);
}

public interface IVerificationEvidenceRepository
{
    IReadOnlyList<VerificationEvidence> GetAll(Guid? endpointContractId = null);
    VerificationEvidence Add(VerificationEvidence evidence);
    void Clear();
}