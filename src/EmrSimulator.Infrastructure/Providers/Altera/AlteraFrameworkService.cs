namespace EmrSimulator.Infrastructure.Providers.Altera;

public sealed class AlteraFrameworkService
{
    public object Operation(string operation) => new { operation, status = "Success", fileBlock = "synthetic" };
}