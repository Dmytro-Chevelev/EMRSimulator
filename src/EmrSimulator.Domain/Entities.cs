using EmrSimulator.Contracts;

namespace EmrSimulator.Domain;

public sealed class EmrProfile : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public EmrProviderType Provider { get; set; }
    public string BaseUrl { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
}

public sealed class Scenario : BaseEntity
{
    public Guid EmrProfileId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ScenarioType ScenarioType { get; set; }
    public bool IsActive { get; set; }
    public string Seed { get; set; } = string.Empty;
}

public sealed class Patient : BaseEntity
{
    public string ExternalPatientId { get; set; } = string.Empty;
    public string Mrn { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
}

public sealed class Appointment : BaseEntity
{
    public Guid PatientId { get; set; }
    public DateTime StartTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string Status { get; set; } = "Scheduled";
}

public sealed class Order : BaseEntity
{
    public Guid PatientId { get; set; }
    public string OrderType { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public DateTime PlacedAtUtc { get; set; } = DateTime.UtcNow;
}

public sealed class Result : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid? OrderId { get; set; }
    public string ResultType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public DateTime ResultedAtUtc { get; set; } = DateTime.UtcNow;
}

public sealed class RequestLog : BaseEntity
{
    public string Provider { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string RequestHeadersJson { get; set; } = "{}";
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public int ResponseCode { get; set; }
    public int DurationMs { get; set; }
    public Guid? ScenarioId { get; set; }
}
