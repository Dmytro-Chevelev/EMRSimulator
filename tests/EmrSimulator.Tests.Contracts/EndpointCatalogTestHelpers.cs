using EmrSimulator.Domain;
using EmrSimulator.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EmrSimulator.Tests.Contracts;

internal static class EndpointCatalogTestHelpers
{
    public static IReadOnlyList<EndpointContract> LoadContracts()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<EmrSimulatorDbContext>()
            .UseSqlite(connection)
            .Options;

        using var dbContext = new EmrSimulatorDbContext(options);
        dbContext.Database.EnsureCreated();

        return new EfEndpointContractRepository(dbContext).GetAll();
    }
}