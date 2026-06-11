using EmrSimulator.Contracts;
using EmrSimulator.Domain;

namespace EmrSimulator.Tests.Contracts.Cerner;

public sealed class CernerEndpointCatalogTests
{
    [Fact]
    public void Catalog_includes_cerner_rest_and_hl7_families()
    {
        var contracts = EndpointCatalogTestHelpers.LoadContracts();

        Assert.Contains(contracts, contract => contract.Provider == EmrProviderType.Cerner && contract.ContractFamily == EndpointContractFamily.CernerVitalsLink);
        Assert.Contains(contracts, contract => contract.Provider == EmrProviderType.Cerner && contract.Protocol == EndpointProtocol.Hl7Mllp);
    }
}