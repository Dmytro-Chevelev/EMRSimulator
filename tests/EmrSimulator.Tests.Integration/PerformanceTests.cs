using System.Diagnostics;
using System.Net.Http.Headers;
using EmrSimulator.Infrastructure.Hl7;

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

    [Fact]
    public async Task Native_http_and_soap_p95_response_is_under_one_second()
    {
        using var client = factory.CreateClient();
        var samples = new List<double>();

        for (var sampleNumber = 0; sampleNumber < 10; sampleNumber++)
        {
            samples.Add(await MeasureAsync(() => client.GetAsync("/Midmark?launch=perf&iss=issuer")));
            samples.Add(await MeasureAsync(() => client.GetAsync("/FHIR/R4/Patient/EP-1001")));
            samples.Add(await MeasureAsync(() => client.PostAsync("/Unity/UnityService.svc", SoapContent("Magic"))));
        }

        var p95 = Percentile(samples, 0.95);
        Assert.True(p95 < 1000, $"Native HTTP/SOAP p95 response time was {p95:F2} ms, expected under 1000 ms.");
    }

    [Fact]
    public void Hl7_ack_generation_p95_response_is_under_one_second()
    {
        var service = new Hl7MllpService();
        var samples = new List<double>();
        var stopwatch = new Stopwatch();

        for (var sampleNumber = 0; sampleNumber < 30; sampleNumber++)
        {
            stopwatch.Restart();
            var ack = service.HandleMessage("\u000bMSH|^~\\&|SRC|LOC|DST|LOC|202501010101||ADT^A01|CTRL-1|P|2.5\r\u001c\r");
            stopwatch.Stop();

            Assert.Contains("MSA|AA", ack, StringComparison.OrdinalIgnoreCase);
            samples.Add(stopwatch.Elapsed.TotalMilliseconds);
        }

        var p95 = Percentile(samples, 0.95);
        Assert.True(p95 < 1000, $"HL7 ACK p95 response time was {p95:F2} ms, expected under 1000 ms.");
    }

    private static async Task<double> MeasureAsync(Func<Task<HttpResponseMessage>> request)
    {
        var stopwatch = Stopwatch.StartNew();
        using var response = await request();
        stopwatch.Stop();

        response.EnsureSuccessStatusCode();
        return stopwatch.Elapsed.TotalMilliseconds;
    }

    private static StringContent SoapContent(string action)
    {
        var content = new StringContent($"<s:Envelope><s:Body><{action} /></s:Body></s:Envelope>");
        content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
        content.Headers.Add("SOAPAction", action);
        return content;
    }

    private static double Percentile(IReadOnlyList<double> samples, double percentile)
    {
        var ordered = samples.OrderBy(sample => sample).ToList();
        var index = Math.Min(ordered.Count - 1, (int)Math.Ceiling(percentile * ordered.Count) - 1);
        return ordered[index];
    }
}
