namespace EmrSimulator.Domain;

public sealed class SyntheticCredentialSet : BaseEntity
{
    public Guid EmrProfileId { get; set; }
    public string CredentialName { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecretHashOrMarker { get; set; } = "synthetic-secret";
    public string Username { get; set; } = string.Empty;
    public string PasswordHashOrMarker { get; set; } = "synthetic-password";
    public string BearerToken { get; set; } = string.Empty;
    public string BasicAuthUser { get; set; } = string.Empty;
    public string BasicAuthPasswordHashOrMarker { get; set; } = "synthetic-basic-password";
    public string TenantId { get; set; } = string.Empty;
    public string TenantShortName { get; set; } = string.Empty;
    public DateTime? TokenExpiresAtUtc { get; set; }
    public DateTime? RetiredAtUtc { get; set; }
    public bool IsDefaultSynthetic { get; set; } = true;
}