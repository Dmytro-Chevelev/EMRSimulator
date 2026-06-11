# Cerner Smoke Test

## Local Endpoints

- `POST /VitalsLink/login`
- `GET /VitalsLink/patients/CE-1001`
- `POST /VitalsLink/devices/register`
- `GET /api/v1/cerner/physicians`
- HL7 MLLP listener: `127.0.0.1:2575`

## Verification

- Contract catalog coverage: `tests/EmrSimulator.Tests.Contracts/Cerner/CernerEndpointCatalogTests.cs`
- VitalsLink workflow: `tests/EmrSimulator.Tests.Integration/Cerner/VitalsLinkWorkflowTests.cs`
- HL7 framing/ACK: `tests/EmrSimulator.Tests.Integration/Cerner/Hl7MllpListenerTests.cs`