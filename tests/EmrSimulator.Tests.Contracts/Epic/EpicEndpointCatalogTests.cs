using EmrSimulator.Contracts;
using EmrSimulator.Domain;

namespace EmrSimulator.Tests.Contracts.Epic;

public sealed class EpicEndpointCatalogTests
{
    [Fact]
    public void Catalog_includes_epic_native_endpoint_families()
    {
        var contracts = EndpointCatalogTestHelpers.LoadContracts();

        Assert.Contains(contracts, contract => contract.Provider == EmrProviderType.Epic && contract.ContractFamily == EndpointContractFamily.EpicLaunch);
        Assert.Contains(contracts, contract => contract.Provider == EmrProviderType.Epic && contract.ContractFamily == EndpointContractFamily.EpicFhir);
        Assert.Contains(contracts, contract => contract.Provider == EmrProviderType.Epic && contract.ContractFamily == EndpointContractFamily.EpicReports);
    }
}