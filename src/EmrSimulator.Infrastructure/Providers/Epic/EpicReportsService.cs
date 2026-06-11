using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Infrastructure.Providers.Epic;

public sealed class EpicReportsService
{
    public object List(string patientId) => new[] { EpicSampleBuilder.Report($"RPT-{patientId}") };

    public object Save() => EpicSampleBuilder.Report("RPT-SAVED-1001");

    public object Get(string reportId) => EpicSampleBuilder.Report(reportId);
}