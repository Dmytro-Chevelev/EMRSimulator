using EmrSimulator.Application;
using Microsoft.Extensions.DependencyInjection;

namespace EmrSimulator.Tests.Integration;

public class PatientImportJsonTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public void Json_source_name_can_be_recorded_for_import_report()
    {
        using var scope = factory.Services.CreateScope();
        var facade = scope.ServiceProvider.GetRequiredService<IEmrSimulatorFacade>();
        var jsonLike = "EP-2001,MRN-2001,Avery,North,1990-05-01,Female";

        var report = facade.ImportPatients("json", jsonLike);

        Assert.Equal("json", report.SourceFormat);
        Assert.Equal(1, report.AcceptedCount);
    }
}
