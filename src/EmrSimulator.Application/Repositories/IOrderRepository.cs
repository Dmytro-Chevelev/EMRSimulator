using EmrSimulator.Domain;

namespace EmrSimulator.Application.Repositories;

public interface IOrderRepository
{
    IReadOnlyList<Order> GetAll();
    IReadOnlyList<Order> GetByPatientId(Guid patientId);
}
