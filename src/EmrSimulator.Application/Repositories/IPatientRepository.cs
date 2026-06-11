using EmrSimulator.Domain;

namespace EmrSimulator.Application.Repositories;

public interface IPatientRepository
{
    IReadOnlyList<Patient> GetAll();
    Patient? GetById(Guid id);
    void Add(Patient patient);
    bool ExistsByMrn(string mrn);
    bool ExistsByExternalId(string externalPatientId);
}
