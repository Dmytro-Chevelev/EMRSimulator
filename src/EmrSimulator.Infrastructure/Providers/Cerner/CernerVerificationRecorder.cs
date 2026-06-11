namespace EmrSimulator.Infrastructure.Providers.Cerner;

public sealed class CernerVerificationRecorder
{
    public object Record(string route) => new { provider = "Cerner", route, verified = true };
}