using EmrSimulator.Application.Repositories;
using EmrSimulator.Domain;

namespace EmrSimulator.Infrastructure.Persistence;

public sealed class EfAppointmentRepository(EmrSimulatorDbContext dbContext) : IAppointmentRepository
{
    public IReadOnlyList<Appointment> GetAll()
        => dbContext.Appointments
            .OrderBy(a => a.StartTimeUtc)
            .ToList();

    public IReadOnlyList<Appointment> GetByPatientId(Guid patientId)
        => dbContext.Appointments
            .Where(a => a.PatientId == patientId)
            .OrderBy(a => a.StartTimeUtc)
            .ToList();
}
