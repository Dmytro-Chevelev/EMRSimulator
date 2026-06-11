# Unity And Framework Smoke Test

## Local Endpoints

- `POST /Unity/UnityService.svc`
- `POST /IQFrameworkWebService/IQConnectIF.asmx`
- `POST /Unity/GetSecurityToken`
- `POST /Altera/ReturnMagicJSON`
- `GET /Xbap/{routeName}.aspx`

## Verification

- Athena catalog coverage: `tests/EmrSimulator.Tests.Contracts/Unity/AthenaEndpointCatalogTests.cs`
- Altera catalog coverage: `tests/EmrSimulator.Tests.Contracts/Unity/AlteraEndpointCatalogTests.cs`
- Unity SOAP: `tests/EmrSimulator.Tests.Integration/Unity/UnitySoapEnvelopeTests.cs`
- Altera ASMX: `tests/EmrSimulator.Tests.Integration/Unity/AlteraFrameworkAsmxTests.cs`