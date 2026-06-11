# Data Model: External EMR Endpoint Simulator

## Overview

This feature extends the existing simulator model with persistent contract coverage, provider-specific native endpoint dispatch, synthetic auth, saved workflow state, and verification evidence. All data is synthetic and local. EF Core Fluent API remains the persistence mapping strategy.

## Entities

### EndpointContract

Represents one documented endpoint, SOAP operation, HL7 boundary, or data-source boundary from the source documents.

**Fields**:

- `Id`: stable identifier
- `Provider`: Epic, Cerner, Altera, AthenaFlow, AthenaServer, or shared Midmark-facing provider family
- `ContractFamily`: EpicLaunch, EpicOAuth, EpicFHIR, EpicReports, EpicDevices, AthenaUnity, AthenaDataSource, AlteraUnity, AlteraFramework, AlteraBrowserRoute, CernerVitalsLink, CernerHL7, CernerMidmarkService
- `Direction`: connector-to-simulator, simulator-to-provider simulation, bidirectional message boundary, or data-source simulation
- `Protocol`: HTTP_REST, HTTP_FHIR, SOAP_XML, ASMX_SOAP, HL7_MLLP, DATA_SOURCE
- `Method`: HTTP method when applicable
- `PathPattern`: exact path or route template when applicable
- `ActionName`: SOAP action, Unity Magic action, ASMX method, HL7 message type, or data-source operation name when applicable
- `Purpose`: human-readable purpose from source inventory
- `RequestContractName`: documented request shape name
- `ResponseContractName`: documented response shape name
- `AuthRequired`: true when synthetic credentials/tokens/headers are required
- `AcceptedSerializerVariants`: documented variants such as PascalCase, camelCase, string enum, numeric enum, SOAP envelope variant, or MLLP framing variant
- `SupportStatus`: Planned, Implemented, Verified, Failed, Deferred
- `SourceDocument`: `.docs/external-emr-endpoints.md` or `.docs/external-emr-api-contracts.md`
- `SourceAnchor`: section/table reference or stable source label

**Validation rules**:

- `Provider`, `ContractFamily`, `Protocol`, `Purpose`, and `SupportStatus` are required.
- HTTP contracts require either `PathPattern` or an explicit reason if the source is data-source/message only.
- SOAP/ASMX contracts require `ActionName` or operation name.
- HL7 contracts require message direction and expected message family.
- A documented contract may not be marked `Verified` unless at least one verification evidence record passes.

**Relationships**:

- One `EndpointContract` has many `ContractExample` records.
- One `EndpointContract` has many `VerificationEvidence` records.
- One `EndpointContract` has many `RequestLog` records through route/action identity.

### ProviderProfile

Feature term for the existing `EmrProfile` aggregate extended with native contract configuration. Implementation should not create a parallel provider-profile root entity unless a later design change explicitly retires or replaces `EmrProfile`.

**Fields**:

- Existing `EmrProfile` identity and display fields: `Id`, `Name`, `Provider`, `Enabled`, and `BaseUrl`
- `NativeBaseUrl`: local connector-facing base address for HTTP/SOAP paths
- `Hl7Host`: local HL7 listener host when applicable
- `Hl7Port`: local HL7 listener port when applicable
- `SyntheticCredentialSetId`
- `ActiveScenarioId`
- `ResetGeneration`: incremented when operator reset clears persisted synthetic state

**Validation rules**:

- Provider family must be one of the supported provider types.
- HTTP/SOAP profiles require a native base URL.
- Cerner HL7 profile requires configured host and port when HL7 support is enabled.
- Credentials must be synthetic and must not contain real secrets.

**Relationships**:

- One existing `EmrProfile` owns many `SyntheticScenario` records through provider-profile configuration.
- One existing `EmrProfile` can own one active synthetic credential set.

### SyntheticCredentialSet

Represents provider-compatible synthetic credentials, tokens, and header expectations.

**Fields**:

- `Id`
- `ProviderProfileId`
- `CredentialName`
- `ClientId`
- `ClientSecretHashOrMarker`
- `Username`
- `PasswordHashOrMarker`
- `BearerToken`
- `BasicAuthUser`
- `BasicAuthPasswordHashOrMarker`
- `TenantId`
- `TenantShortName`
- `TokenExpiresAtUtc`
- `RetiredAtUtc`
- `IsDefaultSynthetic`

**Validation rules**:

- Default values must be synthetic and documented for local use.
- Real provider secrets are invalid for default scenarios.
- Protected requests must match the provider-compatible credential or token pattern for the selected profile and scenario.

### SyntheticScenario

Extends existing scenario behavior with persisted provider workflow state.

**Fields**:

- `Id`
- `ProviderProfileId`
- `Name`
- `ScenarioType`: HappyPath, PatientNotFound, InvalidCredentials, Unauthorized, Timeout, ServerError, RateLimited, MalformedResponse, UnsupportedOperation, PartialBinaryTransfer, ProviderUnavailable
- `Seed`
- `IsActive`
- `ResetGeneration`
- `DefaultResponseMode`

**Validation rules**:

- Same request plus same scenario state produces the same response.
- Scenario data must be synthetic.
- Scenario transitions must preserve related patient/report/device/message identifiers.

**Relationships**:

- One scenario owns many persisted workflow state records and verification evidence records.

### SyntheticPatientGraph

Represents a coherent patient-centered synthetic data set for connector workflows.

**Fields**:

- `Id`
- `ScenarioId`
- `PatientId`
- `ExternalPatientId`
- `Mrn`
- `ProviderSpecificIdentifiersJson`
- `DemographicsJson`
- `EncounterJson`
- `ProviderJson`
- `VitalsJson`
- `FHIRJson`
- `UnityXml`
- `AdtHl7Message`

**Validation rules**:

- Must not contain PHI.
- Required patient identifiers must be present for each enabled provider family.
- Provider-specific identifiers must be stable within the scenario until reset.

### SyntheticReportState

Represents reports, PDFs, binary data, diagnostic reports, data files, calibration reports, and document images generated or saved by connector workflows.

**Fields**:

- `Id`
- `ScenarioId`
- `ProviderProfileId`
- `PatientGraphId`
- `ReportId`
- `ReportType`
- `DeviceId`
- `Status`
- `ReportMetadataJson`
- `ReportDataBase64`
- `ReportRawDataBase64`
- `DiscreteDataXml`
- `PdfBase64`
- `FhirDiagnosticReportJson`
- `DataFileBlocksJson`
- `CreatedByEndpointContractId`
- `UpdatedByEndpointContractId`

**Validation rules**:

- Save/retrieve/report-list operations must reference the same persisted report identifiers.
- Binary/file block reads validate offset and completion state.
- Reset removes generated reports unless reseeded.

### DeviceRegistrationState

Represents registered device instances, heartbeat state, start-test state, abort state, and calibration device context.

**Fields**:

- `Id`
- `ScenarioId`
- `ProviderProfileId`
- `DeviceId`
- `InstanceId`
- `DisplayName`
- `DeviceType`
- `Connected`
- `LastHeartbeatAtUtc`
- `ActiveWorkflowJson`
- `CalibrationStateJson`

**Validation rules**:

- Duplicate registration returns deterministic success or documented conflict behavior based on scenario.
- Heartbeat requires known synthetic device identifiers.
- Abort and calibration operations update state consistently.

### DocumentState

Represents Unity/ASMX document operations and data-source document metadata.

**Fields**:

- `Id`
- `ScenarioId`
- `ProviderProfileId`
- `PatientGraphId`
- `AccessionNumber`
- `DocumentType`
- `DocumentMetadataXml`
- `DocumentImageBase64`
- `SourceOperation`

**Validation rules**:

- Document-by-accession must return saved or seeded document state.
- Document image save updates persisted state until reset.

### Hl7MessageState

Represents inbound and outbound HL7 messages and acknowledgements.

**Fields**:

- `Id`
- `ScenarioId`
- `ProviderProfileId`
- `Direction`: Inbound or Outbound
- `MessageType`: ADT, ORU, or other documented family
- `ControlId`
- `PatientIdentifier`
- `RawMessage`
- `AckMessage`
- `ValidationStatus`
- `FailureReason`
- `ReceivedAtUtc`
- `SentAtUtc`

**Validation rules**:

- MLLP framing must be validated at the transport boundary.
- Duplicate control IDs produce deterministic duplicate handling.
- Valid inbound messages receive ACK; invalid messages receive documented NAK/failure.

### RequestLogEntry

Extends existing request logs with contract identity and native protocol details.

**Fields**:

- Existing request-log fields: provider, route, method, headers, request body, response body, response code, duration, scenario, created timestamp
- `EndpointContractId`
- `Protocol`
- `ActionName`
- `CorrelationId`
- `ConnectorSessionId`
- `ValidationStatus`
- `AuthOutcome`
- `ResetGeneration`

**Validation rules**:

- Every simulator interaction creates a log entry unless explicitly excluded for health checks.
- Logs must redact or avoid real secrets.
- Logs remain available until operator reset.

### VerificationEvidence

Represents proof that an endpoint contract was exercised and matched expected behavior.

**Fields**:

- `Id`
- `EndpointContractId`
- `ProviderProfileId`
- `ScenarioId`
- `VerificationName`
- `RequestSampleReference`
- `ExpectedOutcome`
- `ActualStatus`
- `ActualResponseSummary`
- `Passed`
- `FailureReason`
- `VerifiedAtUtc`
- `ToolOrTestName`

**Validation rules**:

- Required for marking endpoint contracts as `Verified`.
- Must identify success/failure and scenario.
- Must not store real patient data.

## State Transitions

### EndpointContract

```text
Planned -> Implemented -> Verified
Planned -> Implemented -> Failed
Implemented -> Failed -> Verified
Implemented -> Deferred
```

### SyntheticScenario

```text
Inactive -> Active -> Inactive
Active -> ResetRequested -> Inactive/Active with incremented ResetGeneration
```

### SyntheticReportState

```text
Seeded -> Retrieved
Seeded -> Reviewed -> Signed/Reviewed
Generated -> Saved -> Retrieved -> Reviewed/Compared/Converted
Generated/Saved -> ResetRemoved
```

### DeviceRegistrationState

```text
Unregistered -> Registered -> HeartbeatReceived -> ActiveWorkflow -> Aborted/Completed
Registered -> Removed
Registered -> ResetRemoved
```

### Hl7MessageState

```text
Received -> Validated -> Acknowledged
Received -> Rejected -> NegativeAcknowledged
PreparedOutbound -> Sent -> AckRecorded/Failed
```

## Reset Behavior

Operator reset clears generated workflow state, request logs, and verification evidence for the selected scope, increments `ResetGeneration`, and reseeds deterministic default synthetic data. Reset must not delete endpoint contract definitions or default provider profiles.
