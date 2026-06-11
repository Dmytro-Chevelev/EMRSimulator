using EmrSimulator.Contracts.Unity;

namespace EmrSimulator.Infrastructure.Providers.Athena;

public sealed class AthenaUnityService
{
    public UnityTokenResponse Token() => UnitySampleBuilder.Token();

    public UnityOperationResponse Magic(string action) => UnitySampleBuilder.Operation(action);
}