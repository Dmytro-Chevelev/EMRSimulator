namespace EmrSimulator.Infrastructure.Hl7;

public sealed class Hl7MllpService
{
    public const char StartBlock = '\u000b';
    public const char EndBlock = '\u001c';

    public string HandleMessage(string frame)
    {
        var message = frame.Trim(StartBlock, EndBlock, '\r', '\n');
        var controlId = ExtractControlId(message);
        return $"{StartBlock}MSH|^~\\&|SIM|MIDMARK|CONNECTOR|LOCAL|{DateTime.UtcNow:yyyyMMddHHmmss}||ACK|ACK-{controlId}|P|2.5\rMSA|AA|{controlId}\r{EndBlock}\r";
    }

    private static string ExtractControlId(string message)
    {
        var msh = message.Split('\r', '\n').FirstOrDefault(segment => segment.StartsWith("MSH", StringComparison.OrdinalIgnoreCase));
        var parts = msh?.Split('|');
        return parts is { Length: > 9 } ? parts[9] : "SYNTHETIC";
    }
}