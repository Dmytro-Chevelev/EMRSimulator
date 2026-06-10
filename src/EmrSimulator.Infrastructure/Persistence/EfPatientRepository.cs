using EmrSimulator.Application.Repositories;
using EmrSimulator.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmrSimulator.Infrastructure.Persistence;

public sealed class EfPatientRepository(EmrSimulatorDbContext dbContext) : IPatientRepository
{
    public IReadOnlyList<Patient> GetAll()
        => dbContext.Patients
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToList();

    public Patient? GetById(Guid id)
        => dbContext.Patients.FirstOrDefault(p => p.Id == id);

    public void Add(Patient patient)
    {
        dbContext.Patients.Add(patient);
        dbContext.SaveChanges();
    }

    public bool ExistsByMrn(string mrn)
        => dbContext.Patients.Any(p => p.Mrn == mrn);

    public bool ExistsByExternalId(string externalPatientId)
        => dbContext.Patients.Any(p => p.ExternalPatientId == externalPatientId);
}
