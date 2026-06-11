using EmrSimulator.Domain;

namespace EmrSimulator.Application.Repositories;

public interface IResultRepository
{
    IReadOnlyList<Result> GetAll();
    IReadOnlyList<Result> GetByPatientId(Guid patientId);
    IReadOnlyList<Result> GetByOrderId(Guid orderId);
}
