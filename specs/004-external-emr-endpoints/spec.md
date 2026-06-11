# Feature Specification: External EMR Endpoint Simulator

**Feature Branch**: `004-external-emr-endpoints`  
**Created**: June 11, 2026  
**Status**: Draft  
**Input**: User description: "--files .docs/external-emr-api-contracts.md .docs/external-emr-endpoints.md Using this files we need to have implementation for all endpoints to have ability to connect our EMR to the simulator"

## Clarifications

### Session 2026-06-11

- Q: Which protocol fidelity level is required for connector compatibility? -> A: Native protocol support: REST/FHIR HTTP, SOAP/XML-compatible endpoints, and HL7 TCP/MLLP boundaries
- Q: How long should synthetic simulator state be retained? -> A: Persist synthetic state until operator reset
- Q: How should protected simulator flows handle authentication? -> A: Enforce provider-compatible synthetic credentials, tokens, and auth headers for protected flows
- Q: Which contract validation tolerance should the simulator apply? -> A: Accept documented contract shapes plus known serializer variants such as Pascal/camel case and string/numeric enums
- Q: How should Cerner Midmark ADT patient search source patient data? -> A: Seed 15 default synthetic patients in the database and return all from `/api/v1/cerner/patients`
- Q: How should provider-facing request and response payloads be represented in implementation contracts? -> A: Require typed provider contract DTOs/records for all provider-facing request and response payloads
- Q: How should default patient seeding interact with later synthetic patient imports? -> A: Seed 15 default patients non-destructively and return all current database patients, including later synthetic imports
- Q: How should simulator reset affect synthetic patient data? -> A: Reset restores the 15 default seeded patients and removes imported/generated synthetic patients
- Q: Where should the default local SQLite database be stored? -> A: Store SQLite in a stable repo-local data folder such as `.data/emrsimulator.db`

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Connect Through Epic Workflows (Priority: P1)

As an EMR integration engineer, I want Epic connector workflows to target the simulator for launch, authorization, FHIR resource reads, PDF conversion, reports, device actions, authentication, and launcher registration so that Epic-facing integrations can be exercised without a live Epic environment.

**Why this priority**: Epic is the most complete inventoried provider surface and includes launch, authorization, FHIR, report, and device flows that prove the simulator can support a full EMR-to-device workflow.

**Independent Test**: Can be tested by configuring an Epic-style connector profile to use the simulator and completing launch, token, patient/report lookup, report save/retrieve, device start/abort, PDF conversion, and close workflows with only synthetic data.

**Acceptance Scenarios**:

1. **Given** a configured Epic simulator profile and a synthetic launch context, **When** the connector calls the documented launch and callback routes, **Then** the simulator returns a valid launch session, token response, and workflow context matching the documented contract shapes.
2. **Given** a valid Epic simulator session, **When** the connector requests patient, observation, diagnostic report, binary PDF, report list, report save, report data file, review, compare, convert, device start, device abort, authentication, and launcher registration operations, **Then** each documented endpoint returns a contract-valid response tied to the same synthetic patient and report scenario.
3. **Given** the connector sends an unsupported Epic patient, report, device, or authorization value, **When** the request reaches the simulator, **Then** the simulator returns a documented failure outcome and records the request for troubleshooting.

---

### User Story 2 - Connect Through Cerner Workflows (Priority: P2)

As an EMR integration engineer, I want Cerner CareAware/VitalsLink, HL7, and Midmark-facing Cerner service workflows to target the simulator so that patient, encounter, vitals, device, physician, ADT, and result messaging can be tested without a live Cerner environment.

**Why this priority**: Cerner covers both request/response endpoint workflows and message-based HL7 workflows, which are required to validate the simulator beyond simple REST-style calls.

**Independent Test**: Can be tested by configuring a Cerner-style connector profile to authenticate, resolve barcodes, retrieve locations, encounters, patients, register and heartbeat a device, post vitals, submit HL7 messages, and search/retrieve ADT patients against the simulator.

**Acceptance Scenarios**:

1. **Given** a configured Cerner simulator profile, **When** the connector calls authentication, barcode format, personnel barcode, location, encounter, patient, device registration, heartbeat, vitals posting, and device removal operations, **Then** the simulator returns the documented contract shapes and consistent synthetic identifiers across the workflow.
2. **Given** a configured HL7 scenario, **When** the connector sends or receives supported ADT and ORU-style messages through the native TCP/MLLP simulator boundary, **Then** the simulator acknowledges valid messages, preserves the patient/report relationship, and records message details for review.
3. **Given** a Midmark-facing Cerner service client and the default synthetic database seed, **When** it searches ADT patients through `/api/v1/cerner/patients`, **Then** the simulator returns all current synthetic patient records from the database, including the 15 default seeded patients and any later synthetic imports, according to the documented service contract.
4. **Given** a Midmark-facing Cerner service client, **When** it retrieves an ADT patient, updates last access, lists physicians, or submits HL7 messages, **Then** the simulator responds according to the documented service contracts.

---

### User Story 3 - Connect Through Unity and Framework Workflows (Priority: P3)

As an EMR integration engineer, I want Athena/Centricity and Altera/Allscripts Unity and framework-style workflows to target the simulator so that token, patient, provider, clinical summary, document, settings, report, calibration, and launcher flows can be tested without live Unity or framework services.

**Why this priority**: Athena and Altera integrations use service-operation contracts rather than fixed REST paths, and the simulator must support those provider families to satisfy the complete endpoint inventory.

**Independent Test**: Can be tested by configuring Athena/Centricity and Altera/Allscripts connector profiles to call documented Unity operations and framework operations, then verifying contract-valid XML, JSON, file-block, URL, report, calibration, and settings responses.

**Acceptance Scenarios**:

1. **Given** a configured Athena/Centricity simulator profile, **When** the connector requests a security token, patient, providers, allergies, medications, problems, document image save, document by accession, document list, document type, or configured data-source patient/search/report data, **Then** the simulator returns contract-valid synthetic responses for each documented operation.
2. **Given** a configured Altera/Allscripts simulator profile, **When** the connector calls Unity token, validation, Magic, ReturnMagicJSON, retire-token, or patient/context operations, **Then** the simulator returns contract-valid responses matching the documented operation names and action semantics.
3. **Given** a Midmark framework-style client, **When** it calls documented framework operations for plugins, report managers, files, reports, XBAP preparation, calibration reports, settings, common settings, provider lists, or notifications, **Then** the simulator returns the documented response shape and records the operation.

---

### User Story 4 - Configure and Verify Endpoint Coverage (Priority: P4)

As a simulator operator, I want to configure provider scenarios and inspect endpoint activity so that I can prove every documented endpoint is implemented, troubleshoot connector behavior, and choose realistic success or failure responses.

**Why this priority**: Operators need clear evidence that endpoint coverage is complete and usable, but this value depends on the endpoint families being represented first.

**Independent Test**: Can be tested by loading the endpoint inventory, selecting a provider scenario, sending representative requests for every endpoint group, and reviewing the resulting coverage and request log evidence.

**Acceptance Scenarios**:

1. **Given** the endpoint inventory from the source documents, **When** an operator views simulator coverage, **Then** every documented endpoint, operation, message boundary, and data-source boundary is listed with provider, direction, purpose, contract family, and support status.
2. **Given** a configured scenario, **When** the operator selects success, missing data, authorization failure, malformed payload, or unavailable-provider behavior, **Then** subsequent connector requests receive the selected response type and are logged with enough detail to diagnose the connector interaction.
3. **Given** completed endpoint tests, **When** the operator reviews the verification evidence, **Then** the simulator identifies which documented endpoints passed, failed, or remain untested.

### Edge Cases

- Connector sends valid endpoint requests in an unsupported provider profile.
- Connector sends malformed JSON, malformed XML, invalid form data, invalid query values, missing headers, missing credentials, expired tokens, unknown patients, unknown reports, unknown devices, or unknown encounters.
- Connector sends real credentials, mismatched synthetic credentials, invalid authorization headers, retired Unity tokens, expired OAuth tokens, or invalid VitalsLink authentication values to protected simulator flows.
- Connector requests binary report/PDF/file blocks with invalid offsets, empty payloads, partial payloads, or unsupported file types.
- Connector repeats registration, save, heartbeat, abort, notification, or message-submit operations and expects idempotent or clearly reported outcomes.
- Connector sends HL7 messages with unsupported message types, invalid framing, missing patient identifiers, duplicate control IDs, or unreachable peer settings.
- Connector calls SOAP/WCF/ASMX-style operations with an unknown action name, missing token, retired token, malformed XML input, or unexpected parameter count.
- Connector sends a request that uses known serializer variants such as PascalCase versus camelCase property names or string versus numeric enum values.
- Connector requests a configured data-source simulation where the selected source profile has no matching patient, provider, document, report, or clinical summary data.
- Multiple connector sessions call the same provider profile concurrently and must not corrupt another session's patient, report, token, device, or message state.
- Operator resets persisted simulator state while connector sessions are inactive, after which prior generated reports, device registrations, messages, request logs, and verification evidence are no longer returned.
- Operator resets persisted simulator state after importing or generating additional synthetic patients, after which the patient database is restored to the 15 default seeded synthetic patients.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST maintain a coverage inventory for every endpoint, operation, message boundary, and data-source boundary documented in `.docs/external-emr-endpoints.md` and `.docs/external-emr-api-contracts.md`, including provider, direction, method or protocol, path or action pattern, purpose, contract family, and support status.
- **FR-002**: System MUST provide separate simulator profiles for Epic, Athena/Centricity, Altera/Allscripts, and Cerner so connector requests can be routed to provider-appropriate scenarios and response contracts.
- **FR-003**: System MUST support the documented Epic launch, close, OAuth discovery, authorization, token, backend token, FHIR resource, backend FHIR, PDF conversion, report, device, authentication, and launcher registration workflows.
- **FR-004**: System MUST support Epic report and device workflow contracts for report lists, report lookup, report save, report data file retrieval, report review, report comparison, report conversion, device start test, and device abort operations.
- **FR-005**: System MUST support Athena/Centricity Unity workflows for security-token retrieval, patient retrieval, provider retrieval, clinical summaries for allergies, medications, and problems, document image save, document by accession, document list, document type metadata, and configured CPS/CEMR data-source simulations.
- **FR-006**: System MUST support Altera/Allscripts Unity workflows for security-token retrieval, valid-token retrieval, JSON token retrieval, token retirement, Magic operations, ReturnMagicJSON operations, and documented patient/context actions.
- **FR-007**: System MUST support Altera Midmark framework-style workflows for plugin folders, report manager folders, configuration and settings folders, plugin lookup, file download, server file versions, report audit, workflow notifications, report lists, report retrieval, report deletion, report existence checks, XBAP preparation, data file operations, calibration report operations, settings operations, common settings, and provider-list retrieval.
- **FR-008**: System MUST support the documented Altera browser/client workflow routes for launcher, test, review, compare, and calibration scenarios with deterministic synthetic URLs or outcomes.
- **FR-009**: System MUST support Cerner CareAware/VitalsLink workflows for authentication, barcode formats, personnel barcode resolution, location hierarchy, encounter search, encounter retrieval, patient retrieval, device registration, device heartbeat, discrete vitals posting, and device removal.
- **FR-010**: System MUST support Cerner HL7 workflows for inbound ADT-style patient messages, outbound ORU/result messages, acknowledgement behavior, and failure handling for invalid messages.
- **FR-011**: System MUST support Cerner Midmark-facing service workflows for ADT patient search, ADT patient retrieval, patient last-access update, physician list retrieval, HL7 message submission, and pending-test HL7 message submission. The `/api/v1/cerner/patients` ADT patient search MUST return all current synthetic patient records in the database, including the 15 default seeded patients and any later synthetic imports.
- **FR-012**: System MUST validate incoming requests against the documented contract families and return contract-valid success or failure responses for token, patient, provider, observation, diagnostic report, binary/PDF, report, device, Unity XML, Unity JSON, ASMX/framework, VitalsLink, ADT patient, physician, vitals, and HL7 message shapes. Provider-facing request and response payloads MUST be represented by typed provider contract DTOs or records rather than generic `object` or anonymous payload shapes.
- **FR-013**: System MUST preserve synthetic scenario consistency across a connector session so patients, encounters, providers, reports, devices, tokens, documents, vitals, and HL7 identifiers remain coherent across related endpoint calls.
- **FR-014**: System MUST allow operators to select deterministic success and failure scenarios for each provider profile, including at minimum success, authorization failure, not found, malformed request, unsupported operation, unavailable provider, and partial binary/file transfer outcomes.
- **FR-015**: System MUST record every simulator interaction with timestamp, provider profile, endpoint or operation, request identifiers, response outcome, correlation/session identifier when available, and enough request/response metadata to troubleshoot without storing real patient data.
- **FR-016**: System MUST expose endpoint coverage and verification evidence so operators can see which documented endpoints are implemented, tested, passing, failing, or not yet exercised.
- **FR-017**: System MUST use only synthetic patient, provider, encounter, report, device, document, vitals, calibration, settings, and message data in default scenarios, including exactly 15 default synthetic patient records in the non-destructive database seed.
- **FR-018**: System MUST document how connector teams configure each provider family to target the simulator and which source-document endpoints are covered by the implementation.
- **FR-019**: System MUST expose native protocol-compatible simulator boundaries for the documented connector surfaces, including REST/FHIR HTTP calls, SOAP/XML-compatible WCF or ASMX-style operations, and HL7 TCP/MLLP message exchange.
- **FR-020**: System MUST persist synthetic simulator state, including generated identifiers, imported/generated synthetic patients, saved reports, device registrations, documents, settings, messages, request logs, and verification evidence, until an operator explicitly resets that state. Reset MUST restore the 15 default seeded synthetic patients and remove imported or generated synthetic patient records. The default local SQLite database MUST use a stable repo-local data folder such as `.data/emrsimulator.db` so API launch working directory does not change the active simulator database.
- **FR-021**: System MUST enforce provider-compatible synthetic credentials, tokens, and authentication headers for protected simulator flows, including Epic OAuth/FHIR, Unity security-token flows, VitalsLink authentication, protected framework calls, and any documented protected report, device, or message workflow.
- **FR-022**: System MUST accept the documented contract shapes plus known connector serializer variants, including PascalCase and camelCase property names and string or numeric enum representations, while rejecting requests that omit required identifiers or contain invalid required structures.

### Key Entities *(include if feature involves data)*

- **Endpoint Contract**: A documented endpoint, operation, message boundary, or data-source boundary with provider, direction, method/protocol, path/action/pattern, purpose, request shape, response shape, accepted serializer variants, native protocol expectation, and support status.
- **Provider Profile**: A selectable simulator configuration for Epic, Athena/Centricity, Altera/Allscripts, or Cerner that controls supported contract families, synthetic credentials, response behavior, and default scenario data.
- **Synthetic Scenario**: A coherent set of synthetic patients, providers, encounters, reports, devices, documents, observations, vitals, calibration records, settings, tokens, and messages used to produce deterministic responses and retained until operator reset.
- **Connector Session**: A related set of requests from an EMR connector, tracked across launch, token, report, device, Unity, framework, VitalsLink, HL7, and data-source style interactions.
- **Request Log Entry**: A troubleshooting record for a simulator interaction, including endpoint/operation identity, provider profile, request metadata, response outcome, correlation/session identifiers, and validation results.
- **Verification Evidence**: A record that a documented endpoint or operation was exercised and whether the observed request and response matched the expected contract.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% of endpoint, operation, message boundary, and data-source boundary entries from the two source documents have a corresponding simulator coverage entry and verification status.
- **SC-002**: For each provider family, a connector engineer can complete at least one representative end-to-end synthetic workflow against the simulator in under 15 minutes without using a live EMR system.
- **SC-003**: At least 95% of valid representative requests in the verification suite receive contract-valid responses within 1 second in the standard local simulator environment.
- **SC-004**: 100% of malformed, unauthorized, unsupported, and not-found verification cases return documented failure outcomes and are visible in request logs within 5 seconds.
- **SC-005**: Operators can identify implemented, passing, failing, and untested endpoint coverage for all provider families within 2 minutes of opening the verification evidence.
- **SC-006**: Default simulator scenarios contain zero real patient records, real provider records, real report payloads, or real credentials.
- **SC-007**: Connector setup documentation enables a new engineer to configure at least one provider profile and perform a successful smoke workflow in under 30 minutes.
- **SC-008**: 100% of imported/generated synthetic patients, generated synthetic reports, device registrations, documents, messages, request logs, and verification evidence created during a verification run remain available after simulator restart until an operator reset is performed.
- **SC-009**: 100% of protected-flow verification cases accept configured synthetic credentials and reject missing, invalid, expired, retired, or real credentials with documented failure outcomes.
- **SC-010**: 100% of contract-validation verification cases accept documented shapes and known serializer variants while rejecting requests with missing required identifiers or invalid required structures.

## Assumptions

- The source documents `.docs/external-emr-endpoints.md` and `.docs/external-emr-api-contracts.md` are the authoritative endpoint and contract inventory for this feature.
- Endpoint compatibility means the simulator accepts the documented request patterns and returns contract-valid synthetic responses; it does not require connecting to real Epic, Athena/Centricity, Altera/Allscripts, Cerner, SQL Server, Oracle, or interface-engine systems.
- SOAP/WCF/ASMX and HL7 boundaries are in scope as connector-facing contract simulations, even when their source systems do not expose fixed JSON REST endpoints.
- Connector compatibility requires native protocol support for REST/FHIR HTTP, SOAP/XML-compatible WCF or ASMX-style operations, and HL7 TCP/MLLP message exchange; HTTP-only test substitutes are not sufficient for this feature.
- Provider database boundaries are represented as configurable data-source simulations with synthetic data, not as real database deployments.
- Authentication compatibility uses synthetic credentials, tokens, and headers only; real provider credentials and real secrets are out of scope and must not be stored in default scenarios.
- Contract compatibility includes known connector serializer differences from the source documents, including property-name casing and enum representation differences.
- Default scenarios prioritize deterministic connector testing over exhaustive clinical realism.
- Administrative management and verification views may reuse existing simulator concepts for providers, scenarios, request logs, and smoke evidence.
- Local development uses a stable repo-local SQLite database path such as `.data/emrsimulator.db`; operators may override the connection string explicitly when needed.
