using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Infrastructure.Providers.Epic;

public sealed class EpicVerificationRecorder
{
    public EpicVerificationRecordResponse Record(string route) => new("Epic", route, true);
}