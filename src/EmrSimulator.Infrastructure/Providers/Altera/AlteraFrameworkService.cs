using EmrSimulator.Contracts.Unity;

namespace EmrSimulator.Infrastructure.Providers.Altera;

public sealed class AlteraFrameworkService
{
    public UnityFrameworkOperationResponse Operation(string operation) => new(operation, "Success", "synthetic");
}