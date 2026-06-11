using EmrSimulator.Contracts.Unity;

namespace EmrSimulator.Infrastructure.Providers.Athena;

public sealed class AthenaUnityService
{
    public object Token() => UnitySampleBuilder.Token();

    public object Magic(string action) => UnitySampleBuilder.Operation(action);
}