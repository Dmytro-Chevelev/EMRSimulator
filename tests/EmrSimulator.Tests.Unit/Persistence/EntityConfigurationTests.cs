using EmrSimulator.Domain;
using EmrSimulator.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EmrSimulator.Tests.Unit.Persistence;

public sealed class EntityConfigurationTests
{
    [Fact]
    public void Patients_have_unique_index_on_mrn()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<EmrSimulatorDbContext>()
            .UseSqlite(connection)
            .Options;

        using var db = new EmrSimulatorDbContext(options);
        db.Database.EnsureCreated();

        var patientEntity = db.Model.FindEntityType(typeof(Patient));
        var mrnIndex = patientEntity?.GetIndexes().SingleOrDefault(i =>
            i.Properties.Count == 1 && i.Properties[0].Name == nameof(Patient.Mrn));

        Assert.NotNull(mrnIndex);
        Assert.True(mrnIndex!.IsUnique);
    }

    [Fact]
    public void Scenarios_fk_to_profiles_uses_cascade_delete()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<EmrSimulatorDbContext>()
            .UseSqlite(connection)
            .Options;

        using var db = new EmrSimulatorDbContext(options);
        db.Database.EnsureCreated();

        var scenarioEntity = db.Model.FindEntityType(typeof(Scenario));
        var fk = scenarioEntity?.GetForeignKeys().SingleOrDefault(f => f.PrincipalEntityType.ClrType == typeof(EmrProfile));

        Assert.NotNull(fk);
        Assert.Equal(DeleteBehavior.Cascade, fk!.DeleteBehavior);
    }

    [Fact]
    public void Request_logs_allow_nullable_scenario_fk()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<EmrSimulatorDbContext>()
            .UseSqlite(connection)
            .Options;

        using var db = new EmrSimulatorDbContext(options);
        db.Database.EnsureCreated();

        var requestLogEntity = db.Model.FindEntityType(typeof(RequestLog));
        var scenarioFkProperty = requestLogEntity?.FindProperty(nameof(RequestLog.ScenarioId));

        Assert.NotNull(scenarioFkProperty);
        Assert.True(scenarioFkProperty!.IsNullable);
    }

    [Fact]
    public void External_emr_entities_are_mapped_with_catalog_key_index()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<EmrSimulatorDbContext>()
            .UseSqlite(connection)
            .Options;

        using var db = new EmrSimulatorDbContext(options);
        db.Database.EnsureCreated();

        var endpointEntity = db.Model.FindEntityType(typeof(EndpointContract));
        var keyIndex = endpointEntity?.GetIndexes().SingleOrDefault(index =>
            index.Properties.Count == 1 && index.Properties[0].Name == nameof(EndpointContract.ContractKey));

        Assert.NotNull(endpointEntity);
        Assert.NotNull(keyIndex);
        Assert.True(keyIndex!.IsUnique);
        Assert.NotNull(db.Model.FindEntityType(typeof(SyntheticCredentialSet)));
        Assert.NotNull(db.Model.FindEntityType(typeof(SyntheticReportState)));
        Assert.NotNull(db.Model.FindEntityType(typeof(Hl7MessageState)));
        Assert.NotNull(db.Model.FindEntityType(typeof(VerificationEvidence)));
    }
}
