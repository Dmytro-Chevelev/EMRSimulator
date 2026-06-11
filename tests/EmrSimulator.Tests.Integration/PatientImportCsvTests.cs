using EmrSimulator.Application;
using EmrSimulator.Infrastructure;

namespace EmrSimulator.Tests.Integration;

public class PatientImportCsvTests
{
    [Fact]
    public void Csv_import_rejects_duplicates()
    {
        var facade = new EmrSimulatorFacade(new InMemoryEmrSimulatorStore());
        var csv = "EP-1001,MRN-1001,Jordan,Casey,1980-04-20,Unknown";

        var report = facade.ImportPatients("csv", csv);

        Assert.Equal(0, report.AcceptedCount);
        Assert.Equal(1, report.RejectedCount);
    }
}
