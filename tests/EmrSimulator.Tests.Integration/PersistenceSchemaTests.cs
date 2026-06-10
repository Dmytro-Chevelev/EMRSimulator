using EmrSimulator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmrSimulator.Tests.Integration;

public sealed class PersistenceSchemaTests
{
    [Fact]
    public void Ensure_created_builds_expected_tables()
    {
        var dbPath = Path.Combine(Path.GetTempPath(), $"emr-simulator-schema-{Guid.NewGuid():N}.db");

        try
        {
            var options = new DbContextOptionsBuilder<EmrSimulatorDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            using (var db = new EmrSimulatorDbContext(options))
            {
                db.Database.EnsureCreated();

                using var command = db.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table';";

                if (command.Connection?.State != System.Data.ConnectionState.Open)
                {
                    command.Connection?.Open();
                }

                using var reader = command.ExecuteReader();
                var tableNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                while (reader.Read())
                {
                    tableNames.Add(reader.GetString(0));
                }

                Assert.Contains("Patients", tableNames);
                Assert.Contains("Scenarios", tableNames);
                Assert.Contains("RequestLogs", tableNames);
                Assert.Contains("MockResponses", tableNames);
                Assert.Contains("Appointments", tableNames);
                Assert.Contains("Orders", tableNames);
                Assert.Contains("Results", tableNames);
            }
        }
        finally
        {
            if (File.Exists(dbPath))
            {
                try
                {
                    File.Delete(dbPath);
                }
                catch (IOException)
                {
                    // Some SQLite providers keep a transient lock on file teardown.
                    // Cleanup is best-effort and should not fail the schema assertion.
                }
            }
        }
    }
}
