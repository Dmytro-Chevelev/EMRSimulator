using EmrSimulator.Application.Repositories;
using EmrSimulator.Domain;

namespace EmrSimulator.Infrastructure.Persistence;

public sealed class EfResultRepository(EmrSimulatorDbContext dbContext) : IResultRepository
{
    public IReadOnlyList<Result> GetAll()
        => dbContext.Results
            .OrderByDescending(r => r.ResultedAtUtc)
            .ToList();

    public IReadOnlyList<Result> GetByPatientId(Guid patientId)
        => dbContext.Results
            .Where(r => r.PatientId == patientId)
            .OrderByDescending(r => r.ResultedAtUtc)
            .ToList();

    public IReadOnlyList<Result> GetByOrderId(Guid orderId)
        => dbContext.Results
            .Where(r => r.OrderId == orderId)
            .OrderByDescending(r => r.ResultedAtUtc)
            .ToList();
}
