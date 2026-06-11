using EmrSimulator.Application.Repositories;
using EmrSimulator.Domain;

namespace EmrSimulator.Infrastructure.Persistence;

public sealed class EfOrderRepository(EmrSimulatorDbContext dbContext) : IOrderRepository
{
    public IReadOnlyList<Order> GetAll()
        => dbContext.Orders
            .OrderBy(o => o.PlacedAtUtc)
            .ToList();

    public IReadOnlyList<Order> GetByPatientId(Guid patientId)
        => dbContext.Orders
            .Where(o => o.PatientId == patientId)
            .OrderBy(o => o.PlacedAtUtc)
            .ToList();
}
