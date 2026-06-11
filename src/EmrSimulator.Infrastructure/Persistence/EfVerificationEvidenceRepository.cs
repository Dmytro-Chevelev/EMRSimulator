using EmrSimulator.Application.Repositories;
using EmrSimulator.Domain;

namespace EmrSimulator.Infrastructure.Persistence;

public sealed class EfVerificationEvidenceRepository(EmrSimulatorDbContext dbContext) : IVerificationEvidenceRepository
{
    public IReadOnlyList<VerificationEvidence> GetAll(Guid? endpointContractId = null)
    {
        var query = dbContext.VerificationEvidence.AsQueryable();
        if (endpointContractId is not null)
        {
            query = query.Where(e => e.EndpointContractId == endpointContractId.Value);
        }

        return query.OrderByDescending(e => e.VerifiedAtUtc).ToList();
    }

    public VerificationEvidence Add(VerificationEvidence evidence)
    {
        dbContext.VerificationEvidence.Add(evidence);
        dbContext.SaveChanges();
        return evidence;
    }

    public void Clear()
    {
        dbContext.VerificationEvidence.RemoveRange(dbContext.VerificationEvidence);
        dbContext.SaveChanges();
    }
}