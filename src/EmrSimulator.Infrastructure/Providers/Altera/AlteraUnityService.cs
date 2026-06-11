using EmrSimulator.Contracts.Unity;

namespace EmrSimulator.Infrastructure.Providers.Altera;

public sealed class AlteraUnityService
{
    public object Token() => UnitySampleBuilder.Token();

    public object Magic(string action) => UnitySampleBuilder.Operation(action);
}