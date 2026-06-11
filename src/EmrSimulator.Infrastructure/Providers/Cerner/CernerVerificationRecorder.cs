using EmrSimulator.Contracts.Cerner;

namespace EmrSimulator.Infrastructure.Providers.Cerner;

public sealed class CernerVerificationRecorder
{
    public CernerVerificationRecordResponse Record(string route) => new("Cerner", route, true);
}