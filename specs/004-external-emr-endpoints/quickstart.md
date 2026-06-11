# Quickstart: External EMR Endpoint Simulator

## Prerequisites

- .NET 8 SDK
- Node.js 20+ and npm for the Admin UI
- PowerShell on Windows
- Existing source documents:
  - `.docs/external-emr-endpoints.md`
  - `.docs/external-emr-api-contracts.md`

## Run Backend API

From the repository root:

```powershell
dotnet run --project src/EmrSimulator.Api/EmrSimulator.Api.csproj
```

Default development URLs:

- API and Swagger: `http://localhost:5288/swagger`
- Admin/control API prefix: `http://localhost:5288/api/v1`

## Run Admin UI

From the Angular workspace root:

```powershell
cd src/EmrSimulator.AdminUi
npm install --legacy-peer-deps
npm start
```

Default Admin UI URL:

- `http://localhost:4200`

The Admin UI dev server proxies `/api` to `http://localhost:5288`.

## Contract Preservation Rule

Do not require connector or bridge changes for documented connector-facing surfaces. Admin/control APIs remain under `/api/v1`, but compatibility surfaces must preserve the documented native paths, SOAP actions, XML envelopes, HL7 framing, auth headers, and payload families wherever existing connectors call them.

Examples:

- Epic launch remains `/Midmark?launch={launchToken}&iss={fhirBaseUrl}`.
- Epic PDF conversion remains `/Pdf/convert?environmentId={environmentId}&userId={userId}&documentId={documentId}`.
- Altera framework remains `/IQFrameworkWebService/IQConnectIF.asmx` with SOAP/XML-compatible method calls.
- Cerner VitalsLink paths remain relative to configured `BASE_URL`, such as `/security/auth/login` and `/cas/api/v1/patients?_id={patientId}`.
- Cerner HL7 uses TCP/MLLP framing rather than an HTTP-only substitute.

## Configure a Connector for Local Smoke Testing

1. Start the backend API.
2. In the Admin UI or control API, select the provider profile and scenario.
3. Point the connector's provider base URL or service endpoint to the simulator's local listener/base URL.
4. Use documented synthetic credentials from the provider profile.
5. Run one connector workflow.
6. Review request logs and endpoint verification evidence in Admin UI or API.

## Smoke Workflow Targets

### Epic

- Launch: `GET /Midmark?launch={syntheticLaunchToken}&iss={simulatedFhirBaseUrl}`
- Callback: `GET /Midmark/Redirect?code={syntheticCode}&state={sessionId}`
- FHIR metadata: `GET {iss}/metadata`
- Token: `POST {TokenURI}`
- Report list/save/retrieve: documented `/api/v1/Reports/...` endpoints
- Device start/abort: `/api/v1/Devices/StartTest`, `/api/v1/Devices/Abort`

Expected result: connector completes launch, token, patient/report lookup, device action, and report retrieval using one coherent synthetic patient/report scenario.

### Cerner

- Login: `POST /security/auth/login`
- Barcode formats/personnel: documented `/cas/api/v1/barcode/...` paths
- Location/encounter/patient: documented `/cas/api/v1/...` paths
- Device registration/heartbeat/removal: documented `/gda/api/devices...` paths
- Vitals posting: `POST /cas/api/v1/chartdoc/discrete`
- HL7: send MLLP-framed ADT message to configured local listener
- Midmark-facing service: `/api/v1/ADTPatients/...`, `/api/v1/Physicians`, `/api/v1/HL7Messages...`

Expected result: connector can authenticate, resolve patient/encounter context, register a device, post vitals, exchange HL7 messages, and retrieve logged evidence.

### Athena/Centricity

- Unity service endpoint: configured simulator `{unityEndpoint}`
- Operations: `GetSecurityToken`, `Magic` actions for patient, providers, clinical summaries, documents, and document types
- Data-source simulation: configured CPS/CEMR patient/search/report data without real SQL Server or Oracle

Expected result: connector receives SOAP/XML-compatible Unity responses and synthetic data-source responses without live Athena/Centricity dependencies.

### Altera/Allscripts

- Unity service endpoint: configured simulator `{unityEndpoint}`
- SOAP namespace: `http://www.allscripts.com/Unity`
- Operations: token retrieval/validation/retirement, `Magic`, `ReturnMagicJSON`
- Framework endpoint: `/IQFrameworkWebService/IQConnectIF.asmx`
- Browser routes: `/XbapLauncher.aspx`, `/XbapTest.aspx`, `/XbapReview.aspx`, `/XbapCompare.aspx`, `/XbapCalibrate.aspx`

Expected result: connector receives SOAP/XML-compatible Unity/framework responses, deterministic workflow URLs, and persisted synthetic report/settings/calibration state.

## Verification Commands

Backend tests:

```powershell
dotnet test
```

Admin UI build:

```powershell
cd src/EmrSimulator.AdminUi
npm run build
```

Planned feature verification should include:

- Coverage-catalog validation against both source docs
- Contract tests for each provider family
- Integration tests for representative HTTP/FHIR routes
- SOAP/XML request/response smoke tests for Unity and ASMX-style surfaces
- HL7 TCP/MLLP listener smoke tests with ACK/NAK assertions
- Persistence tests proving generated state survives restart until reset
- Auth tests proving synthetic credentials pass and missing/invalid/real credentials fail
- Admin UI build and coverage/log view smoke checks

## Reset Behavior

Use the planned simulator reset operation to clear generated state, request logs, and verification evidence. Reset must not remove endpoint contract definitions or default provider profiles. After reset, deterministic seed data is restored and generated reports, device registrations, messages, documents, settings, and evidence from prior runs are no longer returned.
