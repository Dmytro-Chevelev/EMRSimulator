# Protocol Smoke Samples

## HTTP/FHIR

Use `GET /Midmark`, `POST /oauth2/token`, and representative FHIR read routes against the local simulator host.

Validated by:

- `tests/EmrSimulator.Tests.Integration/Epic/EpicLaunchOAuthTests.cs`
- `tests/EmrSimulator.Tests.Integration/Epic/EpicReportDeviceWorkflowTests.cs`
- `tests/EmrSimulator.Tests.Contracts/Epic/EpicFhirContractTests.cs`

## SOAP/XML

Use Unity or ASMX SOAP envelopes with documented operation names such as `GetSecurityToken`, `Magic`, and `ReturnMagicJSON`.

Validated by:

- `tests/EmrSimulator.Tests.Integration/Unity/UnitySoapEnvelopeTests.cs`
- `tests/EmrSimulator.Tests.Integration/Unity/AlteraFrameworkAsmxTests.cs`

## HL7 TCP/MLLP

Send a synthetic ADT message with MLLP framing to the configured local listener and verify ACK/NAK behavior.

Validated by:

- `tests/EmrSimulator.Tests.Integration/Cerner/Hl7MllpListenerTests.cs`