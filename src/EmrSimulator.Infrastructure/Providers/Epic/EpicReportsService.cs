using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Infrastructure.Providers.Epic;

public sealed class EpicReportsService
{
    public IReadOnlyList<EpicReportResponse> List(string patientId) => [EpicSampleBuilder.Report($"RPT-{patientId}")];

    public EpicReportResponse Save() => EpicSampleBuilder.Report("RPT-SAVED-1001");

    public EpicReportResponse Get(string reportId) => EpicSampleBuilder.Report(reportId);
}