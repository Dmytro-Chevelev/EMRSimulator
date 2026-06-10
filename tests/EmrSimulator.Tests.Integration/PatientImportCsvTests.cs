using EmrSimulator.Application;
using Microsoft.Extensions.DependencyInjection;

namespace EmrSimulator.Tests.Integration;

public class PatientImportCsvTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public void Csv_import_rejects_duplicates()
    {
        using var scope = factory.Services.CreateScope();
        var facade = scope.ServiceProvider.GetRequiredService<IEmrSimulatorFacade>();
        var csv = "EP-1001,MRN-1001,Jordan,Casey,1980-04-20,Unknown";

        var report = facade.ImportPatients("csv", csv);

        Assert.Equal(0, report.AcceptedCount);
        Assert.Equal(1, report.RejectedCount);
    }
}
