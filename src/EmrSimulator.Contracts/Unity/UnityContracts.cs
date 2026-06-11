namespace EmrSimulator.Contracts.Unity;

public sealed record UnityTokenResponse(string SecurityToken, string Status);

public sealed record UnityOperationResponse(string Operation, string Status, string Payload);

public sealed record UnityBrowserRouteResponse(string Route, string Url, string Status);

public sealed record UnityFrameworkOperationResponse(string Operation, string Status, string FileBlock);

public sealed record UnityVerificationRecordResponse(string Provider, string Operation, bool Verified);

public static class UnitySampleBuilder
{
    public static UnityTokenResponse Token() => new("synthetic-unity-token", "Valid");

    public static UnityOperationResponse Operation(string operation)
        => new(operation, "Success", $"<SyntheticResult operation=\"{operation}\" patientId=\"EP-1001\" />");

    public static string SoapEnvelope(string operation)
        => $"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><{operation}Response><{operation}Result>synthetic</{operation}Result></{operation}Response></s:Body></s:Envelope>";
}