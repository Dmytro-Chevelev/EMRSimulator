# Operator Coverage Smoke Test

## Workflow

1. Start the API with `dotnet run --project src/EmrSimulator.Api/EmrSimulator.Api.csproj`.
2. Start the Admin UI from `src/EmrSimulator.AdminUi` with `npm start`.
3. Open `http://localhost:4200/compatibility`.
4. Filter endpoint coverage by provider and protocol.
5. Open evidence for a representative endpoint.
6. Use **Reset State** and verify a reset generation message is returned.

## Verification

- API coverage: `tests/EmrSimulator.Tests.Integration/Admin/EndpointCoverageApiTests.cs`
- Reset API: `tests/EmrSimulator.Tests.Integration/Admin/SimulatorResetApiTests.cs`
- Evidence API: `tests/EmrSimulator.Tests.Integration/Admin/VerificationEvidenceApiTests.cs`
- Admin UI build: `npm run build`