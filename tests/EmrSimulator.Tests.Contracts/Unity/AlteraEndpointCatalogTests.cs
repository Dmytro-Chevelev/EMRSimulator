using EmrSimulator.Contracts;
using EmrSimulator.Domain;

namespace EmrSimulator.Tests.Contracts.Unity;

public sealed class AlteraEndpointCatalogTests
{
    [Fact]
    public void Catalog_includes_altera_framework_contracts()
    {
        var contracts = EndpointCatalogTestHelpers.LoadContracts();

        Assert.Contains(contracts, contract => contract.Provider == EmrProviderType.Altera && contract.ContractFamily == EndpointContractFamily.AlteraFramework);
    }
}