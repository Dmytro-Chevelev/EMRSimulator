using EmrSimulator.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;

namespace EmrSimulator.Tests.Unit.Persistence;

public sealed class SqliteConnectionStringResolverTests
{
    [Fact]
    public void Resolve_default_connection_string_uses_repo_local_data_folder()
    {
        var repoRoot = Path.Combine(Path.GetTempPath(), $"emr-sim-{Guid.NewGuid():N}");
        var contentRoot = Path.Combine(repoRoot, "src", "EmrSimulator.Api");
        Directory.CreateDirectory(contentRoot);
        File.WriteAllText(Path.Combine(repoRoot, "EmrSimulator.slnx"), string.Empty);

        try
        {
            var resolved = SqliteConnectionStringResolver.Resolve(null, contentRoot);
            var builder = new SqliteConnectionStringBuilder(resolved);

            Assert.Equal(Path.Combine(repoRoot, ".data", "emrsimulator.db"), builder.DataSource);
            Assert.True(Directory.Exists(Path.Combine(repoRoot, ".data")));
        }
        finally
        {
            Directory.Delete(repoRoot, recursive: true);
        }
    }

    [Fact]
    public void Resolve_preserves_in_memory_database_connection_strings()
    {
        const string connectionString = "DataSource=:memory:";

        var resolved = SqliteConnectionStringResolver.Resolve(connectionString, Directory.GetCurrentDirectory());

        Assert.Equal(connectionString, resolved);
    }
}