using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmrSimulator.Infrastructure.Persistence;

public sealed class EmrSimulatorDbContext : DbContext
{
    public EmrSimulatorDbContext(DbContextOptions<EmrSimulatorDbContext> options) : base(options) { }

    public DbSet<EmrProfile> EmrProfiles => Set<EmrProfile>();
    public DbSet<Scenario> Scenarios => Set<Scenario>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Result> Results => Set<Result>();
    public DbSet<MockResponse> MockResponses => Set<MockResponse>();
    public DbSet<RequestLog> RequestLogs => Set<RequestLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmrSimulatorDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
