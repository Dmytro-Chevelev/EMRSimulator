using EmrSimulator.Infrastructure.Providers.Cerner;
using EmrSimulator.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EmrSimulator.Tests.Contracts.Cerner;

public sealed class CernerMidmarkServiceTests
{
    [Fact]
    public void Cerner_midmark_service_returns_physician_directory_payload()
    {
        using var fixture = CernerMidmarkServiceFixture.Create();
        var service = new CernerMidmarkService(fixture.DbContext);

        var physicians = service.Physicians();

        Assert.Contains(physicians, physician => physician.Id == "PHY-1001" && physician.Active);
    }

    [Fact]
    public void Cerner_midmark_service_returns_seeded_database_patients()
    {
        using var fixture = CernerMidmarkServiceFixture.Create();
        var service = new CernerMidmarkService(fixture.DbContext);

        var patients = service.SearchPatients();

        Assert.Equal(15, patients.Count);
        Assert.Contains(patients, patient => patient.Id == "ADT-1001");
        Assert.Contains(patients, patient => patient.Mrn == "MRN-1015");
    }

    private sealed class CernerMidmarkServiceFixture : IDisposable
    {
        private readonly SqliteConnection _connection;

        private CernerMidmarkServiceFixture(SqliteConnection connection, EmrSimulatorDbContext dbContext)
        {
            _connection = connection;
            DbContext = dbContext;
        }

        public EmrSimulatorDbContext DbContext { get; }

        public static CernerMidmarkServiceFixture Create()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<EmrSimulatorDbContext>()
                .UseSqlite(connection)
                .Options;

            var dbContext = new EmrSimulatorDbContext(options);
            dbContext.Database.EnsureCreated();
            SyntheticPatientSeeder.EnsureSeeded(dbContext);

            return new CernerMidmarkServiceFixture(connection, dbContext);
        }

        public void Dispose()
        {
            DbContext.Dispose();
            _connection.Dispose();
        }
    }
}