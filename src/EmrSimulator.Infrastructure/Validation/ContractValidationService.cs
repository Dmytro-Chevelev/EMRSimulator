using System.Text.Json;
using EmrSimulator.Application;
using EmrSimulator.Contracts;

namespace EmrSimulator.Infrastructure.Validation;

public sealed class ContractValidationService : IContractValidationService
{
    public ContractValidationResult Validate(string contractFamily, string? payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            return new ContractValidationResult(true, []);
        }

        if (payload.TrimStart().StartsWith('<'))
        {
            return payload.Contains('<') && payload.Contains('>')
                ? new ContractValidationResult(true, [])
                : new ContractValidationResult(false, ["Malformed XML payload"]);
        }

        try
        {
            using var _ = JsonDocument.Parse(payload, new JsonDocumentOptions { AllowTrailingCommas = true });
            return new ContractValidationResult(true, []);
        }
        catch (JsonException ex)
        {
            return new ContractValidationResult(false, [$"Malformed JSON payload: {ex.Message}"]);
        }
    }
}