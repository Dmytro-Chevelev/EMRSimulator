using System.Diagnostics;

namespace EmrSimulator.Tests.Integration;

public sealed class PerformanceTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Provider_route_average_response_is_under_one_second()
    {
        using var client = factory.CreateClient();
        var stopwatch = new Stopwatch();
        var samples = new List<double>(capacity: 10);

        for (var i = 0; i < 10; i++)
        {
            stopwatch.Restart();
            using var response = await client.GetAsync("/api/v1/emr/epic/patients/search");
            stopwatch.Stop();

            response.EnsureSuccessStatusCode();
            samples.Add(stopwatch.Elapsed.TotalMilliseconds);
        }

        var averageMs = samples.Average();
        Assert.True(
            averageMs < 1000,
            $"Average response time was {averageMs:F2} ms, expected under 1000 ms.");
    }
}
