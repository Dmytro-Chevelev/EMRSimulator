using EmrSimulator.Infrastructure.Auth;
using EmrSimulator.Infrastructure.Validation;

namespace EmrSimulator.Tests.Unit;

public sealed class ExternalEmrFoundationTests
{
    [Fact]
    public void Synthetic_auth_rejects_credentials_that_look_real()
    {
        var service = new SyntheticAuthenticationService();

        var result = service.Validate("Epic", "Bearer prod-token");

        Assert.False(result.Authorized);
        Assert.Contains("Real credentials", result.Outcome, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Contract_validation_accepts_json_with_trailing_commas()
    {
        var service = new ContractValidationService();

        var result = service.Validate("Epic", "{ \"patientId\": \"EP-1001\", }");

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}