using EmrSimulator.Application.Repositories;

namespace EmrSimulator.Infrastructure.Persistence;

public sealed class EfSyntheticStateRepository(EmrSimulatorDbContext dbContext) : ISyntheticStateRepository
{
    public int ResetGeneration => dbContext.EmrProfiles.Any()
        ? dbContext.EmrProfiles.Max(p => p.ResetGeneration)
        : 0;

    public int ResetGeneratedState()
    {
        dbContext.SyntheticReportStates.RemoveRange(dbContext.SyntheticReportStates);
        dbContext.DeviceRegistrationStates.RemoveRange(dbContext.DeviceRegistrationStates);
        dbContext.DocumentStates.RemoveRange(dbContext.DocumentStates);
        dbContext.Hl7MessageStates.RemoveRange(dbContext.Hl7MessageStates);
        dbContext.VerificationEvidence.RemoveRange(dbContext.VerificationEvidence);
        dbContext.RequestLogs.RemoveRange(dbContext.RequestLogs);
        dbContext.Patients.RemoveRange(dbContext.Patients);

        var profiles = dbContext.EmrProfiles.ToList();
        foreach (var profile in profiles)
        {
            profile.ResetGeneration++;
            profile.UpdatedAtUtc = DateTime.UtcNow;
        }

        dbContext.SaveChanges();
        SyntheticPatientSeeder.EnsureSeeded(dbContext);

        return profiles.Count == 0 ? 1 : profiles.Max(profile => profile.ResetGeneration);
    }
}