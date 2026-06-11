using EmrSimulator.Contracts;
using EmrSimulator.Domain;

namespace EmrSimulator.Tests.Contracts.Unity;

public sealed class AthenaEndpointCatalogTests
{
    [Fact]
    public void Catalog_includes_athena_unity_contracts()
    {
        var contracts = EndpointCatalogTestHelpers.LoadContracts();

        Assert.Contains(contracts, contract => contract.Provider == EmrProviderType.AthenaFlow && contract.ContractFamily == EndpointContractFamily.AthenaUnity);
    }
}