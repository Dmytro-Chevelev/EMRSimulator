using EmrSimulator.Infrastructure.Hl7;
using EmrSimulator.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmrSimulator.Tests.Integration.Cerner;

public sealed class Hl7MllpListenerTests
{
    [Fact]
    public void Hl7_service_returns_mllp_framed_ack()
    {
        var service = new Hl7MllpService();

        var ack = service.HandleMessage("\u000bMSH|^~\\&|SRC|LOC|DST|LOC|202501010101||ADT^A01|CTRL-1|P|2.5\r\u001c\r");

        Assert.StartsWith(Hl7MllpService.StartBlock.ToString(), ack);
        Assert.Contains("MSA|AA|CTRL-1", ack, StringComparison.OrdinalIgnoreCase);
    }
}

public sealed class Hl7MllpPersistenceTests(SimulatorWebApplicationFactory factory) : IClassFixture<SimulatorWebApplicationFactory>
{
    [Fact]
    public void Hosted_listener_record_path_persists_message_log_and_evidence()
    {
        var hostedService = factory.Services.GetServices<IHostedService>().OfType<Hl7MllpHostedService>().Single();

        hostedService.RecordMessageForVerification(
            "\u000bMSH|^~\\&|SRC|LOC|DST|LOC|202501010101||ADT^A01|CTRL-2|P|2.5\r\u001c\r",
            "\u000bMSH|^~\\&|SIM|MIDMARK|CONNECTOR|LOCAL|202501010101||ACK|ACK-CTRL-2|P|2.5\rMSA|AA|CTRL-2\r\u001c\r");

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EmrSimulatorDbContext>();

        Assert.Contains(dbContext.Hl7MessageStates, message => message.ControlId == "CTRL-2");
        Assert.Contains(dbContext.RequestLogs, log => log.Route == "HL7 MLLP" && log.ResponseCode == 200);
        Assert.Contains(dbContext.VerificationEvidence, item => item.ToolOrTestName == "hl7-mllp-listener");
    }
}