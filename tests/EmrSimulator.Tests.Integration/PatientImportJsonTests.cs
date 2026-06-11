using EmrSimulator.Application;
using EmrSimulator.Infrastructure;

namespace EmrSimulator.Tests.Integration;

public class PatientImportJsonTests
{
    [Fact]
    public void Json_source_name_can_be_recorded_for_import_report()
    {
        var facade = new EmrSimulatorFacade(new InMemoryEmrSimulatorStore());
        var jsonLike = "EP-2001,MRN-2001,Avery,North,1990-05-01,Female";

        var report = facade.ImportPatients("json", jsonLike);

        Assert.Equal("json", report.SourceFormat);
        Assert.Equal(1, report.AcceptedCount);
    }
}
