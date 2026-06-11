using Microsoft.Data.Sqlite;

namespace EmrSimulator.Infrastructure.Persistence;

public static class SqliteConnectionStringResolver
{
    private const string DefaultConnectionString = "Data Source=.data/emrsimulator.db";

    public static string Resolve(string? connectionString, string contentRootPath)
    {
        var configuredConnectionString = string.IsNullOrWhiteSpace(connectionString)
            ? DefaultConnectionString
            : connectionString;
        var builder = new SqliteConnectionStringBuilder(configuredConnectionString);

        if (string.IsNullOrWhiteSpace(builder.DataSource)
            || builder.DataSource.Equals(":memory:", StringComparison.OrdinalIgnoreCase)
            || Path.IsPathFullyQualified(builder.DataSource))
        {
            return configuredConnectionString;
        }

        var rootPath = FindRepositoryRoot(contentRootPath);
        builder.DataSource = Path.GetFullPath(builder.DataSource, rootPath);

        var databaseDirectory = Path.GetDirectoryName(builder.DataSource);
        if (!string.IsNullOrWhiteSpace(databaseDirectory))
        {
            Directory.CreateDirectory(databaseDirectory);
        }

        return builder.ToString();
    }

    private static string FindRepositoryRoot(string startPath)
    {
        var directory = new DirectoryInfo(Path.GetFullPath(startPath));

        while (directory is not null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, ".git"))
                || File.Exists(Path.Combine(directory.FullName, "EmrSimulator.slnx")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        return Path.GetFullPath(startPath);
    }
}