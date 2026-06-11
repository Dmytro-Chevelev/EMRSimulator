using EmrSimulator.Infrastructure.Auth;

namespace EmrSimulator.Tests.Integration.Unity;

public sealed class UnityAuthenticationTests
{
    [Fact]
    public void Unity_auth_rejects_missing_credentials()
    {
        var service = new SyntheticAuthenticationService();

        var result = service.Validate("AthenaFlow", null);

        Assert.False(result.Authorized);
    }
}