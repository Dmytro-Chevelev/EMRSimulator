using EmrSimulator.Application;
using EmrSimulator.Infrastructure;

namespace EmrSimulator.Tests.Contracts;

public class ClinicalRoutesTests
{
    [Fact]
    public void Clinical_data_collections_are_available()
    {
        var facade = new EmrSimulatorFacade(new InMemoryEmrSimulatorStore());

        Assert.NotEmpty(facade.GetAppointments());
        Assert.NotEmpty(facade.GetOrders());
        Assert.NotEmpty(facade.GetResults());
    }
}
