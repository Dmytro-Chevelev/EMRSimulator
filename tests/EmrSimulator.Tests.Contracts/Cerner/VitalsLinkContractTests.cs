using EmrSimulator.Infrastructure.Providers.Cerner;

namespace EmrSimulator.Tests.Contracts.Cerner;

public sealed class VitalsLinkContractTests
{
    [Fact]
    public void Vitals_link_patient_response_contains_synthetic_patient_identity()
    {
        var service = new VitalsLinkClinicalService();

        var patient = service.Patient("CE-1001");

        Assert.Contains("CE-1001", patient.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}