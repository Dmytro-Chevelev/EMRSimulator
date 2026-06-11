using EmrSimulator.Contracts.Unity;

namespace EmrSimulator.Infrastructure.Providers.Unity;

public sealed class UnityVerificationRecorder
{
    public UnityVerificationRecordResponse Record(string operation) => new("Unity", operation, true);
}