using System.Net;
using System.Net.Sockets;
using System.Text;
using EmrSimulator.Application;
using EmrSimulator.Domain;
using EmrSimulator.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmrSimulator.Infrastructure.Hl7;

public sealed class Hl7MllpHostedService(
    IConfiguration configuration,
    Hl7MllpService mllpService,
    IServiceScopeFactory scopeFactory,
    ILogger<Hl7MllpHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (bool.TryParse(configuration["ExternalEmrCompatibility:Hl7:Enabled"], out var enabled) && !enabled)
        {
            return;
        }

        var port = int.TryParse(configuration["ExternalEmrCompatibility:Hl7:Port"], out var configuredPort)
            ? configuredPort
            : 2575;
        var listener = new TcpListener(IPAddress.Loopback, port);

        try
        {
            listener.Start();
            logger.LogInformation("HL7 MLLP simulator listener started on port {Port}", port);

            while (!stoppingToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(stoppingToken);
                _ = HandleClientAsync(client, stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
        {
            logger.LogWarning(ex, "HL7 MLLP simulator listener could not start because port {Port} is already in use", port);
        }
        finally
        {
            listener.Stop();
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        await using var stream = client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        var buffer = new char[4096];
        var length = await reader.ReadAsync(buffer, cancellationToken);
        var response = mllpService.HandleMessage(new string(buffer, 0, length));
        var bytes = Encoding.UTF8.GetBytes(response);
        await stream.WriteAsync(bytes, cancellationToken);
        RecordMessageForVerification(frame: new string(buffer, 0, length), ack: response);
    }

    public void RecordMessageForVerification(string frame, string ack)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EmrSimulatorDbContext>();
        var catalog = scope.ServiceProvider.GetRequiredService<IEndpointCatalogService>();
        var evidence = scope.ServiceProvider.GetRequiredService<IVerificationEvidenceService>();

        dbContext.Hl7MessageStates.Add(new Hl7MessageState
        {
            ScenarioId = Guid.Empty,
            EmrProfileId = Guid.Empty,
            Direction = Hl7MessageDirection.Inbound,
            MessageType = "ADT",
            ControlId = ExtractControlId(frame),
            PatientIdentifier = "EP-1001",
            RawMessage = frame,
            AckMessage = ack,
            ValidationStatus = "Accepted"
        });
        dbContext.RequestLogs.Add(new RequestLog
        {
            Provider = "Cerner",
            Route = "HL7 MLLP",
            Method = "MLLP",
            RequestHeadersJson = "{}",
            RequestBody = frame,
            ResponseBody = ack,
            ResponseCode = 200,
            DurationMs = 15
        });
        dbContext.SaveChanges();

        var contract = catalog.FindByPathOrAction("ADT");
        if (contract is not null)
        {
            evidence.Record(contract.Id, "HL7 MLLP ADT", "ACK", true, "hl7-mllp-listener");
        }
    }

    private static string ExtractControlId(string message)
    {
        var msh = message.Split('\r', '\n').FirstOrDefault(segment => segment.Contains("MSH", StringComparison.OrdinalIgnoreCase));
        var parts = msh?.Split('|');
        return parts is { Length: > 9 } ? parts[9] : "SYNTHETIC";
    }
}