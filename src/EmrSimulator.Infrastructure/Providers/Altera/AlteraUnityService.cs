using EmrSimulator.Contracts.Unity;

namespace EmrSimulator.Infrastructure.Providers.Altera;

public sealed class AlteraUnityService
{
    public UnityTokenResponse Token() => UnitySampleBuilder.Token();

    public UnityOperationResponse Magic(string action) => UnitySampleBuilder.Operation(action);
}