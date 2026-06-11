using System.Text.Json;
using EmrSimulator.Application.Repositories;
using EmrSimulator.Contracts;
using EmrSimulator.Domain;

namespace EmrSimulator.Infrastructure.Persistence;

public sealed class EfEndpointContractRepository(EmrSimulatorDbContext dbContext) : IEndpointContractRepository
{
    public IReadOnlyList<EndpointContract> GetAll()
    {
        EnsureSeeded();
        return dbContext.EndpointContracts
            .OrderBy(e => e.Provider)
            .ThenBy(e => e.ContractFamily)
            .ThenBy(e => e.ContractKey)
            .ToList();
    }

    public EndpointContract? GetById(Guid id)
    {
        EnsureSeeded();
        return dbContext.EndpointContracts.FirstOrDefault(e => e.Id == id);
    }

    public EndpointContract? FindByPathOrAction(string pathOrAction)
    {
        EnsureSeeded();
        return dbContext.EndpointContracts.AsEnumerable().FirstOrDefault(e =>
            Matches(e.PathPattern, pathOrAction) || Matches(e.ActionName, pathOrAction));
    }

    public void UpsertRange(IEnumerable<EndpointContract> contracts)
    {
        foreach (var contract in contracts)
        {
            var existing = dbContext.EndpointContracts.FirstOrDefault(e => e.ContractKey == contract.ContractKey);
            if (existing is null)
            {
                dbContext.EndpointContracts.Add(contract);
                continue;
            }

            existing.Provider = contract.Provider;
            existing.ContractFamily = contract.ContractFamily;
            existing.Direction = contract.Direction;
            existing.Protocol = contract.Protocol;
            existing.Method = contract.Method;
            existing.PathPattern = contract.PathPattern;
            existing.ActionName = contract.ActionName;
            existing.Purpose = contract.Purpose;
            existing.RequestContractName = contract.RequestContractName;
            existing.ResponseContractName = contract.ResponseContractName;
            existing.AuthRequired = contract.AuthRequired;
            existing.AcceptedSerializerVariants = contract.AcceptedSerializerVariants;
            existing.SupportStatus = contract.SupportStatus;
            existing.SourceDocument = contract.SourceDocument;
            existing.SourceAnchor = contract.SourceAnchor;
            existing.UpdatedAtUtc = DateTime.UtcNow;
        }

        dbContext.SaveChanges();
    }

    private void EnsureSeeded()
    {
        if (dbContext.EndpointContracts.Any())
        {
            return;
        }

        UpsertRange(LoadSeedContracts());
    }

    private static IReadOnlyList<EndpointContract> LoadSeedContracts()
    {
        var seedPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "external-emr-endpoint-contracts.json");
        if (!File.Exists(seedPath))
        {
            seedPath = Path.Combine(Directory.GetCurrentDirectory(), "src", "EmrSimulator.Infrastructure", "SeedData", "external-emr-endpoint-contracts.json");
        }

        if (!File.Exists(seedPath))
        {
            return DefaultContracts();
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var rows = JsonSerializer.Deserialize<List<EndpointContractSeed>>(File.ReadAllText(seedPath), options) ?? [];
        return rows.Select(ToContract).ToList();
    }

    private static EndpointContract ToContract(EndpointContractSeed seed)
        => new()
        {
            ContractKey = seed.ContractKey,
            Provider = Enum.Parse<EmrProviderType>(seed.Provider),
            ContractFamily = Enum.Parse<EndpointContractFamily>(seed.ContractFamily),
            Direction = EndpointDirection.ConnectorToSimulator,
            Protocol = Enum.Parse<EndpointProtocol>(seed.Protocol),
            Method = seed.Method,
            PathPattern = seed.PathPattern,
            ActionName = seed.ActionName,
            Purpose = seed.Purpose,
            RequestContractName = seed.RequestContractName ?? seed.ContractKey,
            ResponseContractName = seed.ResponseContractName ?? seed.ContractKey,
            AuthRequired = seed.AuthRequired,
            SupportStatus = EndpointSupportStatus.Implemented,
            SourceDocument = seed.SourceDocument,
            SourceAnchor = seed.ContractKey
        };

    private static IReadOnlyList<EndpointContract> DefaultContracts()
        =>
        [
            CreateDefault("epic-launch", EmrProviderType.Epic, EndpointContractFamily.EpicLaunch, EndpointProtocol.HttpRest, "GET", "/Midmark", "SMART launch entry"),
            CreateDefault("epic-token", EmrProviderType.Epic, EndpointContractFamily.EpicOAuth, EndpointProtocol.HttpRest, "POST", "/oauth2/token", "Synthetic OAuth token exchange"),
            CreateDefault("epic-fhir-patient", EmrProviderType.Epic, EndpointContractFamily.EpicFhir, EndpointProtocol.HttpFhir, "GET", "/FHIR/R4/Patient/{id}", "FHIR patient lookup"),
            CreateDefault("epic-report-save", EmrProviderType.Epic, EndpointContractFamily.EpicReports, EndpointProtocol.HttpRest, "POST", "/api/v1/Reports", "Epic report save"),
            CreateDefault("cerner-vitals-login", EmrProviderType.Cerner, EndpointContractFamily.CernerVitalsLink, EndpointProtocol.HttpRest, "POST", "/VitalsLink/login", "VitalsLink login"),
            CreateDefault("cerner-hl7-adt", EmrProviderType.Cerner, EndpointContractFamily.CernerHl7, EndpointProtocol.Hl7Mllp, null, "127.0.0.1:2575", "HL7 ADT/ORU MLLP acknowledgement"),
            CreateDefault("athena-unity-magic", EmrProviderType.AthenaFlow, EndpointContractFamily.AthenaUnity, EndpointProtocol.SoapXml, "POST", "/Unity/UnityService.svc", "Unity Magic operation"),
            CreateDefault("altera-framework-asmx", EmrProviderType.Altera, EndpointContractFamily.AlteraFramework, EndpointProtocol.AsmxSoap, "POST", "/IQFrameworkWebService/IQConnectIF.asmx", "Framework ASMX operation")
        ];

    private static EndpointContract CreateDefault(
        string contractKey,
        EmrProviderType provider,
        EndpointContractFamily family,
        EndpointProtocol protocol,
        string? method,
        string pathPattern,
        string purpose)
        => new()
        {
            ContractKey = contractKey,
            Provider = provider,
            ContractFamily = family,
            Direction = protocol == EndpointProtocol.Hl7Mllp ? EndpointDirection.BidirectionalMessageBoundary : EndpointDirection.ConnectorToSimulator,
            Protocol = protocol,
            Method = method,
            PathPattern = pathPattern,
            Purpose = purpose,
            RequestContractName = contractKey,
            ResponseContractName = contractKey,
            SupportStatus = EndpointSupportStatus.Implemented,
            SourceDocument = ".docs/external-emr-endpoints.md",
            SourceAnchor = contractKey
        };

    private static bool Matches(string? pattern, string value)
        => !string.IsNullOrWhiteSpace(pattern) && value.Contains(pattern.Split('{')[0], StringComparison.OrdinalIgnoreCase);

    private sealed record EndpointContractSeed(
        string ContractKey,
        string Provider,
        string ContractFamily,
        string Protocol,
        string Purpose,
        bool AuthRequired,
        string SourceDocument,
        string? Method = null,
        string? PathPattern = null,
        string? ActionName = null,
        string? RequestContractName = null,
        string? ResponseContractName = null);
}