using EmrSimulator.Domain;
using EmrSimulator.Infrastructure.Persistence;

namespace EmrSimulator.Infrastructure.Logging;

public sealed class ExternalEmrRequestLogger(InMemoryEmrSimulatorStore store, EmrSimulatorDbContext dbContext)
{
    public void Log(string provider, string route, string method, int statusCode, object? response, string? requestBody = null)
    {
        var log = new RequestLog
        {
            Provider = provider,
            Route = route,
            Method = method,
            RequestHeadersJson = "{}",
            RequestBody = requestBody,
            ResponseBody = response is null ? null : store.Serialize(response),
            ResponseCode = statusCode,
            DurationMs = 15,
            ScenarioId = store.ActiveScenario.Id
        };

        store.AddLog(log);
        dbContext.RequestLogs.Add(new RequestLog
        {
            Id = log.Id,
            Provider = log.Provider,
            Route = log.Route,
            Method = log.Method,
            RequestHeadersJson = log.RequestHeadersJson,
            RequestBody = log.RequestBody,
            ResponseBody = log.ResponseBody,
            ResponseCode = log.ResponseCode,
            DurationMs = log.DurationMs,
            ScenarioId = null,
            CreatedAtUtc = log.CreatedAtUtc,
            UpdatedAtUtc = log.UpdatedAtUtc
        });
        dbContext.SaveChanges();
    }
}