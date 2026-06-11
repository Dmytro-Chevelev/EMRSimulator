namespace EmrSimulator.Infrastructure.Providers.Epic;

public sealed class EpicVerificationRecorder
{
    public object Record(string route) => new { provider = "Epic", route, verified = true };
}