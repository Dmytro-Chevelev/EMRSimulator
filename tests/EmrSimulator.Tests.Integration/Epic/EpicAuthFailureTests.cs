using EmrSimulator.Infrastructure.Auth;

namespace EmrSimulator.Tests.Integration.Epic;

public sealed class EpicAuthFailureTests
{
    [Fact]
    public void Epic_synthetic_auth_rejects_real_credential_markers()
    {
        var service = new SyntheticAuthenticationService();

        var result = service.Validate("Epic", "Bearer real-token");

        Assert.False(result.Authorized);
    }
}