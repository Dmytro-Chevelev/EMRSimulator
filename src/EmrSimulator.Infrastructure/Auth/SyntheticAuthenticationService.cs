using EmrSimulator.Application;
using EmrSimulator.Contracts;

namespace EmrSimulator.Infrastructure.Auth;

public sealed class SyntheticAuthenticationService : ISyntheticAuthenticationService
{
    public SyntheticAuthResult Validate(string provider, string? authorizationHeader, string? token = null)
    {
        var presented = token ?? authorizationHeader;
        if (string.IsNullOrWhiteSpace(presented))
        {
            return new SyntheticAuthResult(false, "Synthetic", "Missing credentials");
        }

            if (presented.Contains("real", StringComparison.OrdinalIgnoreCase)
                || presented.Contains("prod", StringComparison.OrdinalIgnoreCase)
                || presented.Contains("expired", StringComparison.OrdinalIgnoreCase)
                || presented.Contains("retired", StringComparison.OrdinalIgnoreCase))
        {
            return new SyntheticAuthResult(false, "Synthetic", "Real credentials are rejected");
        }

        var authorized = presented.Contains("synthetic", StringComparison.OrdinalIgnoreCase)
                || presented.Contains("test-token", StringComparison.OrdinalIgnoreCase);

        return authorized
            ? new SyntheticAuthResult(true, "Synthetic", "Authorized", presented)
            : new SyntheticAuthResult(false, "Synthetic", $"Invalid {provider} credentials");
    }
}