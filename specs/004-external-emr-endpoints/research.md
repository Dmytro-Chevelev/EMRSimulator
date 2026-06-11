# Research: External EMR Endpoint Simulator

## Decision: Preserve connector-facing contracts as native-compatible simulator surfaces

**Rationale**: The source endpoint inventory is based on existing connectors. Requiring connectors or bridges to change would defeat the feature goal: connect our EMR and existing connector workflows to the simulator. The implementation will expose native-compatible REST/FHIR HTTP routes, SOAP/XML-compatible WCF or ASMX-style operations, and HL7 TCP/MLLP boundaries that match documented paths, actions, headers, framing, and payload families as closely as the local simulator host allows.

**Alternatives considered**:

- HTTP-only compatibility wrappers: rejected because HL7 and SOAP/WCF/ASMX clients may not be able to call substituted test endpoints without connector changes.
- Contract inventory only: rejected because the user needs runnable endpoint implementations.
- Connector-side bridges: rejected because the explicit planning constraint is to keep contracts as-is and avoid connector or bridge changes.

## Decision: Keep admin and control APIs under `/api/v1`; keep native connector paths unmodified

**Rationale**: The constitution requires simulator API routes under `/api/v1`, and the existing Admin UI/API already follow that convention. Connector compatibility routes such as Epic `/Midmark`, `/Pdf/convert`, provider-owned relative FHIR/VitalsLink paths, ASMX-style paths, SOAP actions, and HL7 TCP/MLLP are contract surfaces rather than admin/control APIs. Preserving them is necessary for Provider Contract Fidelity.

**Alternatives considered**:

- Rewrite all connector paths under `/api/v1`: rejected because it would require connector configuration or bridge changes for paths that are documented as native surfaces.
- Expose only `/api/v1/emr/{provider}/...`: rejected because existing coverage is too narrow and does not represent the scanned contracts.

## Decision: Model endpoint coverage as data, then dispatch through provider-specific handlers

**Rationale**: The feature requires 100% coverage traceability from `.docs/external-emr-endpoints.md` and `.docs/external-emr-api-contracts.md`. A persisted endpoint-contract catalog with provider, protocol, direction, path/action, contract family, serializer variants, auth requirement, support status, and verification status allows Admin UI coverage views, contract tests, and deterministic dispatch without scattering endpoint metadata across route handlers.

**Alternatives considered**:

- Hard-code all endpoint metadata in route handlers: rejected because it would be difficult to audit coverage and update docs/tests.
- Generate runtime behavior directly from markdown: rejected because markdown parsing at runtime is brittle; generated or seeded catalog data should be explicit and testable.

## Decision: Add Application interfaces for protocol dispatch and scenario state; implement transports in Infrastructure/Api

**Rationale**: Clean Architecture requires Domain and Application to remain independent of HTTP, SOAP, socket, and persistence details. Application should define contracts for provider endpoint catalog lookup, request validation, synthetic scenario state, response generation, auth validation, request logging, and verification recording. Api and Infrastructure can host ASP.NET Core endpoints, XML/SOAP envelope handlers, and HL7 TCP/MLLP background listeners behind those interfaces.

**Alternatives considered**:

- Put all dispatch in `Program.cs`: rejected because it would violate explicit boundaries and make testing difficult.
- Put transport concerns in Domain: rejected by the constitution.

## Decision: Persist synthetic state in SQLite until operator reset

**Rationale**: The clarified spec requires generated reports, device registrations, documents, settings, messages, request logs, and verification evidence to survive restart until reset. EF Core/SQLite is already the backend storage pattern and supports local offline operation. Reset should be an explicit operator action exposed through admin/control API and UI.

**Alternatives considered**:

- In-memory state only: rejected because it fails restart persistence.
- Session-only state: rejected because end-to-end connector workflows need saved report/message retrieval across calls.
- External database dependency: rejected because the simulator must remain offline/local by default.

## Decision: Use a stable repo-local SQLite database path by default

**Rationale**: A relative SQLite path such as `Data Source=emrsimulator.db` can create different active databases depending on whether the API is launched from the repo root, API project folder, test host, or tooling working directory. The clarified requirement is to use a stable repo-local data folder such as `.data/emrsimulator.db` so local runs, restart persistence checks, and operator reset act on the same simulator database unless an operator explicitly overrides the connection string.

**Alternatives considered**:

- Current-process relative `emrsimulator.db`: rejected because it can make the active DB depend on launch directory and hide seeded/imported data.
- API-project-local `App_Data`: rejected because it is less obvious from repo root workflows and can mix source project files with runtime state.
- No default connection string: rejected because the simulator should run locally/offline with minimal setup.

## Decision: Seed default synthetic patients non-destructively and reset to the baseline

**Rationale**: Cerner Midmark ADT patient search must return real database state, not hard-coded stubs. Startup should guarantee 15 default synthetic patients without deleting later synthetic imports. `/api/v1/cerner/patients` returns all current synthetic database patients. Operator reset restores the deterministic 15-patient baseline and removes imported/generated synthetic patient records so connector tests start from a known state.

**Alternatives considered**:

- Exactly 15 total patients at all times: rejected because operator/import workflows need to add synthetic patients for testing.
- Seed only when the database is empty: rejected because a partially missing default set would not self-heal.
- Preserve imported patients on reset: rejected because reset must provide a deterministic baseline.

## Decision: Use typed provider contract DTOs/records for provider-facing payloads

**Rationale**: Provider compatibility depends on documented contract shapes. Returning anonymous or generic `object` payloads makes drift hard to detect and weakens tests. Shared typed provider request/response DTOs or records in `EmrSimulator.Contracts` make route behavior explicit, testable, and aligned with the source inventories.

**Alternatives considered**:

- Allow anonymous provider stubs: rejected because they are not durable contracts and can hide accidental shape changes.
- Type only admin-visible payloads: rejected because connector-facing payloads carry the primary compatibility risk.
- Generate all DTOs at runtime from markdown: rejected because runtime markdown parsing is brittle and less reviewable than explicit contract records.

## Decision: Enforce synthetic authentication for protected flows

**Rationale**: Epic OAuth/FHIR, Unity token flows, VitalsLink authentication, and protected report/device/framework operations need realistic negative and positive authorization behavior. Synthetic credentials and tokens keep test behavior meaningful without storing real secrets.

**Alternatives considered**:

- Accept any credentials: rejected because invalid-credentials and unauthorized scenarios would not exercise real connector error paths.
- Use real credentials: rejected by Synthetic Data Only and Offline by Default.
- Make auth optional by default: rejected because protected flows would be less representative.

## Decision: Use tolerant contract validation for known serializer variants

**Rationale**: The contract source notes JSON keys use public model property names and enum representation may vary between string and numeric values. Existing .NET connectors may send PascalCase or camelCase JSON and string or numeric enums. The simulator should accept known variants while still rejecting missing required identifiers and invalid required structures.

**Alternatives considered**:

- Exact-shape-only validation: rejected because it would fail compatible connector serializer variants.
- Validate only identifiers: rejected because malformed contract bodies would pass without useful feedback.

## Decision: Implement SOAP/WCF/ASMX compatibility incrementally by envelope/action fidelity, with WSDL only where connector startup requires it

**Rationale**: .NET 8 does not provide legacy ASMX hosting out of the box, and full WCF server fidelity may require additional packages and design decisions. The highest compatibility value is accepting the documented endpoint paths, SOAPAction values, XML envelopes, operation names, and response envelopes used by the connectors. WSDL or metadata endpoints should be added for documented or observed connector startup flows that require service metadata.

**Alternatives considered**:

- Full legacy ASMX/WCF runtime clone: rejected because it is heavyweight and likely unnecessary for connector request/response tests.
- JSON-only operation wrappers: rejected because it would force connector or bridge changes.

## Decision: Use a hosted HL7 TCP/MLLP listener/sender boundary with deterministic acknowledgements

**Rationale**: Cerner HL7 support is a native protocol requirement. A hosted TCP listener can accept MLLP-framed ADT messages, return ACK/NAK responses, persist message records, and feed the same scenario engine used by REST/SOAP. Outbound ORU/result behavior can be represented through a configured sender boundary and verification harness without relying on an external interface engine.

**Alternatives considered**:

- HTTP submit-only HL7 endpoint: rejected because it does not satisfy native TCP/MLLP compatibility.
- External interface engine dependency: rejected because normal simulator use must be offline/local.

## Decision: Keep Admin UI as coverage, scenario, reset, and log operator surface

**Rationale**: The Admin UI should not become a connector bridge. Its role is to show endpoint coverage, configure provider profiles and scenarios, reset persisted synthetic state, and inspect request logs and verification evidence. Connector traffic should hit native simulator boundaries directly.

**Alternatives considered**:

- Add Admin UI-driven connector adapters: rejected because the feature goal is endpoint implementation, not a bridge layer.
- Skip UI changes: rejected because the spec requires operators to inspect coverage and verification status quickly.
