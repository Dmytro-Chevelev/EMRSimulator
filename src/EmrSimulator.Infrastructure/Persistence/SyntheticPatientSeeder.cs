using EmrSimulator.Domain;

namespace EmrSimulator.Infrastructure.Persistence;

public static class SyntheticPatientSeeder
{
    public static void EnsureSeeded(EmrSimulatorDbContext dbContext)
    {
        var existingExternalIds = dbContext.Patients
            .Select(patient => patient.ExternalPatientId)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var existingMrns = dbContext.Patients
            .Select(patient => patient.Mrn)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var patientsToAdd = SeedPatients()
            .Where(patient => !existingExternalIds.Contains(patient.ExternalPatientId) && !existingMrns.Contains(patient.Mrn))
            .ToList();

        if (patientsToAdd.Count == 0)
        {
            return;
        }

        dbContext.Patients.AddRange(patientsToAdd);
        dbContext.SaveChanges();
    }

    private static IReadOnlyList<Patient> SeedPatients() =>
    [
        Patient("ADT-1001", "MRN-1001", "Jordan", "Casey", new DateOnly(1980, 4, 20), "Unknown", "555-0101", "jordan.casey@example.invalid"),
        Patient("ADT-1002", "MRN-1002", "Taylor", "Morgan", new DateOnly(1975, 8, 12), "Female", "555-0102", "taylor.morgan@example.invalid"),
        Patient("ADT-1003", "MRN-1003", "Riley", "Patel", new DateOnly(1992, 1, 5), "Male", "555-0103", "riley.patel@example.invalid"),
        Patient("ADT-1004", "MRN-1004", "Avery", "Nguyen", new DateOnly(1988, 11, 30), "Female", "555-0104", "avery.nguyen@example.invalid"),
        Patient("ADT-1005", "MRN-1005", "Quinn", "Rivera", new DateOnly(1969, 6, 18), "Male", "555-0105", "quinn.rivera@example.invalid"),
        Patient("ADT-1006", "MRN-1006", "Morgan", "Lee", new DateOnly(2001, 2, 9), "Female", "555-0106", "morgan.lee@example.invalid"),
        Patient("ADT-1007", "MRN-1007", "Casey", "Brown", new DateOnly(1957, 9, 24), "Male", "555-0107", "casey.brown@example.invalid"),
        Patient("ADT-1008", "MRN-1008", "Jamie", "Davis", new DateOnly(1995, 12, 3), "Female", "555-0108", "jamie.davis@example.invalid"),
        Patient("ADT-1009", "MRN-1009", "Cameron", "Wilson", new DateOnly(1983, 7, 14), "Male", "555-0109", "cameron.wilson@example.invalid"),
        Patient("ADT-1010", "MRN-1010", "Drew", "Martinez", new DateOnly(1978, 3, 27), "Female", "555-0110", "drew.martinez@example.invalid"),
        Patient("ADT-1011", "MRN-1011", "Sam", "Anderson", new DateOnly(1964, 10, 6), "Male", "555-0111", "sam.anderson@example.invalid"),
        Patient("ADT-1012", "MRN-1012", "Parker", "Thomas", new DateOnly(1999, 5, 21), "Female", "555-0112", "parker.thomas@example.invalid"),
        Patient("ADT-1013", "MRN-1013", "Skyler", "Moore", new DateOnly(1971, 1, 16), "Male", "555-0113", "skyler.moore@example.invalid"),
        Patient("ADT-1014", "MRN-1014", "Reese", "Jackson", new DateOnly(1986, 8, 2), "Female", "555-0114", "reese.jackson@example.invalid"),
        Patient("ADT-1015", "MRN-1015", "Finley", "White", new DateOnly(1990, 4, 11), "Male", "555-0115", "finley.white@example.invalid")
    ];

    private static Patient Patient(string externalPatientId, string mrn, string firstName, string lastName, DateOnly dateOfBirth, string gender, string phone, string email) => new()
    {
        ExternalPatientId = externalPatientId,
        Mrn = mrn,
        FirstName = firstName,
        LastName = lastName,
        DateOfBirth = dateOfBirth,
        Gender = gender,
        Phone = phone,
        Email = email
    };
}