using EmrSimulator.Infrastructure.Providers.Cerner;
using System.Text.Json;

namespace EmrSimulator.Tests.Contracts.Cerner;

public sealed class CernerMidmarkServiceTests
{
    [Fact]
    public void Cerner_midmark_service_returns_physician_directory_payload()
    {
        var service = new CernerMidmarkService();

        var physicians = service.Physicians();
        var json = JsonSerializer.Serialize(physicians);

        Assert.Contains("PHY-1001", json, StringComparison.OrdinalIgnoreCase);
    }
}