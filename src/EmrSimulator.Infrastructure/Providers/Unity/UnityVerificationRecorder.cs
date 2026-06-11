namespace EmrSimulator.Infrastructure.Providers.Unity;

public sealed class UnityVerificationRecorder
{
    public object Record(string operation) => new { provider = "Unity", operation, verified = true };
}