using EmrSimulator.Contracts.Epic;

namespace EmrSimulator.Infrastructure.Providers.Epic;

public sealed class EpicPdfService
{
    public object Convert(string? documentId) => EpicSampleBuilder.Report(documentId ?? "RPT-1001");
}