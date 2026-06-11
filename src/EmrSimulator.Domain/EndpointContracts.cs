using EmrSimulator.Contracts;

namespace EmrSimulator.Domain;

public enum EndpointContractFamily
{
    EpicLaunch,
    EpicOAuth,
    EpicFhir,
    EpicReports,
    EpicDevices,
    AthenaUnity,
    AthenaDataSource,
    AlteraUnity,
    AlteraFramework,
    AlteraBrowserRoute,
    CernerVitalsLink,
    CernerHl7,
    CernerMidmarkService,
    AdminControl
}

public enum EndpointDirection
{
    ConnectorToSimulator,
    SimulatorToProviderSimulation,
    BidirectionalMessageBoundary,
    DataSourceSimulation
}

public enum EndpointProtocol
{
    HttpRest,
    HttpFhir,
    SoapXml,
    AsmxSoap,
    Hl7Mllp,
    DataSource
}

public enum EndpointSupportStatus
{
    Planned,
    Implemented,
    Verified,
    Failed,
    Deferred
}

public sealed class EndpointContract : BaseEntity
{
    public string ContractKey { get; set; } = string.Empty;
    public EmrProviderType Provider { get; set; }
    public EndpointContractFamily ContractFamily { get; set; }
    public EndpointDirection Direction { get; set; }
    public EndpointProtocol Protocol { get; set; }
    public string? Method { get; set; }
    public string? PathPattern { get; set; }
    public string? ActionName { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string RequestContractName { get; set; } = string.Empty;
    public string ResponseContractName { get; set; } = string.Empty;
    public bool AuthRequired { get; set; }
    public string AcceptedSerializerVariants { get; set; } = "PascalCase,camelCase,string-enum,numeric-enum";
    public EndpointSupportStatus SupportStatus { get; set; } = EndpointSupportStatus.Planned;
    public string SourceDocument { get; set; } = string.Empty;
    public string SourceAnchor { get; set; } = string.Empty;
}