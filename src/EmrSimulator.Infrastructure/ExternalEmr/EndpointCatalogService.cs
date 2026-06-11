using EmrSimulator.Application;
using EmrSimulator.Application.Repositories;
using EmrSimulator.Contracts;
using EmrSimulator.Domain;

namespace EmrSimulator.Infrastructure.ExternalEmr;

public sealed class EndpointCatalogService(IEndpointContractRepository repository) : IEndpointCatalogService
{
    public IReadOnlyList<EndpointContractDto> GetEndpointContracts()
        => repository.GetAll().Select(ToDto).ToList();

    public EndpointContractDto? FindByPathOrAction(string pathOrAction)
    {
        var contract = repository.FindByPathOrAction(pathOrAction);
        return contract is null ? null : ToDto(contract);
    }

    private static EndpointContractDto ToDto(EndpointContract contract)
        => new(
            contract.Id,
            contract.ContractKey,
            contract.Provider,
            contract.ContractFamily.ToString(),
            contract.Direction.ToString(),
            contract.Protocol.ToString(),
            contract.Method,
            contract.PathPattern,
            contract.ActionName,
            contract.Purpose,
            contract.RequestContractName,
            contract.ResponseContractName,
            contract.AuthRequired,
            contract.AcceptedSerializerVariants,
            contract.SupportStatus.ToString(),
            contract.SourceDocument,
            contract.SourceAnchor);
}