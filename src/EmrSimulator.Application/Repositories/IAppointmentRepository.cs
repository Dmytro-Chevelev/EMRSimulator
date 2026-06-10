using EmrSimulator.Domain;

namespace EmrSimulator.Application.Repositories;

public interface IAppointmentRepository
{
    IReadOnlyList<Appointment> GetAll();
    IReadOnlyList<Appointment> GetByPatientId(Guid patientId);
}
