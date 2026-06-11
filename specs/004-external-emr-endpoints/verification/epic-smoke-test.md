# Epic Smoke Test

## Local Endpoints

- `GET /Midmark?launch=synthetic-launch&iss=http://localhost:5288/FHIR/R4`
- `POST /oauth2/token`
- `GET /FHIR/R4/Patient/EP-1001`
- `POST /api/v1/Reports`
- `POST /api/v1/DeviceWorkflow/start`

## Verification

- Contract catalog coverage: `tests/EmrSimulator.Tests.Contracts/Epic/EpicEndpointCatalogTests.cs`
- Launch/OAuth: `tests/EmrSimulator.Tests.Integration/Epic/EpicLaunchOAuthTests.cs`
- Reports/devices: `tests/EmrSimulator.Tests.Integration/Epic/EpicReportDeviceWorkflowTests.cs`