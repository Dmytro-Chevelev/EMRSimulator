using System.Net.Http.Json;
using EmrSimulator.Contracts;
using EmrSimulator.Contracts.Cerner;

namespace EmrSimulator.Tests.Integration.Admin;

public sealed class SimulatorResetApiTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public async Task Reset_api_returns_generation_result()
    {
        var client = factory.CreateClient();

        var response = await client.PostAsync("/api/v1/simulator/reset", null);
        var result = await response.Content.ReadFromJsonAsync<SimulatorResetResult>();

        response.EnsureSuccessStatusCode();
        Assert.True(result?.ResetGeneration >= 1);
    }

    [Fact]
    public async Task Reset_api_removes_imported_patients_and_restores_default_seed()
    {
        var client = factory.CreateClient();
        await client.PostAsync("/api/v1/simulator/reset", null);
        using var importContent = new StringContent("IMP-9001,MRN-9001,Test,Import,1991-02-03,Unknown");

        var importResponse = await client.PostAsync("/api/v1/import/patients", importContent);
        var patientsWithImport = await client.GetFromJsonAsync<List<CernerMidmarkPatientResponse>>("/api/v1/cerner/patients");

        var resetResponse = await client.PostAsync("/api/v1/simulator/reset", null);
        var patientsAfterReset = await client.GetFromJsonAsync<List<CernerMidmarkPatientResponse>>("/api/v1/cerner/patients");

        importResponse.EnsureSuccessStatusCode();
        resetResponse.EnsureSuccessStatusCode();
        Assert.NotNull(patientsWithImport);
        Assert.NotNull(patientsAfterReset);
        Assert.Contains(patientsWithImport!, patient => patient.Id == "IMP-9001");
        Assert.Equal(15, patientsAfterReset!.Count);
        Assert.DoesNotContain(patientsAfterReset, patient => patient.Id == "IMP-9001" || patient.Mrn == "MRN-9001");
        Assert.Contains(patientsAfterReset, patient => patient.Id == "ADT-1001" && patient.Mrn == "MRN-1001");
    }
}