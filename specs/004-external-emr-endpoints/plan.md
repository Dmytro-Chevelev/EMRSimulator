# Implementation Plan: External EMR Endpoint Simulator

**Branch**: `004-external-emr-endpoints` | **Date**: June 11, 2026 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `specs/004-external-emr-endpoints/spec.md`

## Summary

Implement the documented external EMR compatibility surface so existing Epic, Cerner, Athena/Centricity, and Altera/Allscripts connectors can point at the simulator without connector or bridge changes. The plan preserves the native connector-facing contracts from `.docs/external-emr-endpoints.md` and `.docs/external-emr-api-contracts.md`: REST/FHIR HTTP paths, SOAP/XML-compatible WCF/ASMX-style operations, HL7 TCP/MLLP boundaries, provider-compatible synthetic authentication, tolerant known serializer variants, persisted synthetic state until operator reset, request logging, and verification evidence.

The implementation approach is additive: keep Admin/control APIs versioned under `/api/v1`, add a persisted endpoint-contract catalog and scenario state model, route native compatibility traffic through provider-specific Application interfaces, host HTTP/SOAP-compatible/HL7 transport boundaries in Api/Infrastructure, and expose coverage/log/reset/verification controls through the existing Admin UI pattern.

## Technical Context

**Language/Version**: C# 12 / .NET 8 for backend; TypeScript 5.8.2 / Angular 20.1.0 for Admin UI  
**Primary Dependencies**: ASP.NET Core minimal API, Swashbuckle/OpenAPI, EF Core 8 SQLite, Angular CLI 20.1.0, RxJS 7.8.1; evaluate CoreWCF/SoapCore-style package only if envelope/action-compatible ASP.NET Core handlers cannot satisfy connector calls  
**Storage**: SQLite via EF Core for persisted synthetic state, endpoint catalog, request logs, and verification evidence; in-memory store remains useful for narrow unit tests  
**Testing**: xUnit unit, contract, and integration tests via `dotnet test`; Angular build via `npm run build`; protocol smoke harnesses for SOAP/XML envelopes and HL7 TCP/MLLP ACK/NAK behavior  
**Target Platform**: Local Windows developer workstation, offline by default; backend default URL `http://localhost:5288`; Admin UI default URL `http://localhost:4200`  
**Project Type**: Web service with Admin UI and native protocol compatibility listeners  
**Performance Goals**: 95% of valid representative simulator requests complete in under 1 second locally; malformed/unauthorized/not-found outcomes visible in logs within 5 seconds; endpoint coverage status visible to operators within 2 minutes  
**Constraints**: Synthetic data only; no PHI; no live Epic/Athena/Altera/Cerner/SQL Server/Oracle/interface-engine dependency for normal use; preserve documented connector-facing contracts; native protocol support required for REST/FHIR HTTP, SOAP/XML-compatible operations, and HL7 TCP/MLLP; contract validation accepts Pascal/camel case and string/numeric enum variants; generated state persists until operator reset  
**Scale/Scope**: All endpoint, operation, message-boundary, and data-source-boundary entries from the two source documents; provider families: Epic, Cerner, Athena/Centricity, Altera/Allscripts; representative end-to-end workflow per provider family plus coverage evidence for every documented entry

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle | Gate | Status | Plan Evidence |
| --- | --- | --- | --- |
| Synthetic Data Only and Offline by Default | No PHI, live provider dependency, or real credentials in default scenarios | Pass | Synthetic credential set, synthetic patient graph, offline SQLite state, no live provider calls |
| Provider Contract Fidelity | Preserve documented route paths, SOAP actions, message framing, response shapes, status behavior, and provider quirks | Pass | Native compatibility contract keeps scanned paths/actions; connector-facing surfaces are not rewritten to new bridge APIs |
| Deterministic Scenario Engine | Same request plus same scenario state produces same response and reproducible failure modes | Pass | Scenario state, reset generation, seeded data, persisted report/device/message state |
| Clean Architecture and Explicit Boundaries | Domain/Application do not depend on transport, persistence, UI, or provider host details | Pass | Application interfaces own dispatch/validation contracts; Api/Infrastructure host HTTP/SOAP/HL7 and EF Core implementation |
| Observable, Tested, and Versioned Changes | Tests, request logs, Swagger/OpenAPI, docs, and verification evidence are planned | Pass | Contract/integration/protocol tests, request log extension, verification evidence, compatibility contract, quickstart |

**Route-versioning note**: Admin/control APIs remain under `/api/v1`. Native connector-facing compatibility routes intentionally preserve source-document paths such as `/Midmark`, `/Pdf/convert`, `/IQFrameworkWebService/IQConnectIF.asmx`, VitalsLink relative paths, and HL7 TCP/MLLP because those are provider contract surfaces. This is not treated as a constitution violation; it satisfies Provider Contract Fidelity while keeping simulator management APIs versioned.

## Project Structure

### Documentation (this feature)

```text
specs/004-external-emr-endpoints/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── external-emr-compatibility-contract.md
└── tasks.md              # Created by /speckit.tasks, not by this plan
```

### Source Code (repository root)

```text
src/
├── EmrSimulator.Api/
│   ├── Program.cs                         # Existing minimal API host; add endpoint group registration and native HTTP compatibility route mapping
│   └── Properties/launchSettings.json     # Existing local URL defaults
├── EmrSimulator.Application/
│   ├── ApplicationContracts.cs            # Existing facade; add/use protocol-neutral interfaces for contract catalog, dispatch, auth, reset, evidence
│   └── Repositories/                      # Add repository interfaces for endpoint catalog, persisted state, logs, evidence
├── EmrSimulator.Contracts/
│   └── SimulatorContracts.cs              # Shared DTOs for admin/control API, coverage, logs, reset, verification summaries
├── EmrSimulator.Domain/
│   ├── Entities.cs                        # Existing entities; add endpoint contracts, synthetic credential/state/evidence entities
│   └── BaseEntity.cs
├── EmrSimulator.Infrastructure/
│   ├── EmrSimulatorFacade.cs              # Existing facade; move broad provider route behavior into focused services
│   ├── InMemoryEmrSimulatorStore.cs       # Keep for deterministic unit tests and seeds; avoid making it the only persistence path
│   ├── Persistence/                       # EF Core DbContext, repositories, Fluent configurations for new entities
│   ├── NativeHttp/                        # Planned: REST/FHIR compatibility handlers and path dispatch helpers
│   ├── Soap/                              # Planned: SOAP/XML envelope/action dispatch helpers for Unity and ASMX-style operations
│   └── Hl7/                               # Planned: TCP/MLLP listener/sender, parser, ACK/NAK generation
└── EmrSimulator.AdminUi/
    └── src/                               # Add coverage, verification, reset, scenario/auth configuration views using existing Angular app patterns

tests/
├── EmrSimulator.Tests.Unit/               # Domain/Application scenario, auth, validation, reset, serializer-variant tests
├── EmrSimulator.Tests.Contracts/          # Provider contract catalog and response shape tests for all documented endpoint families
└── EmrSimulator.Tests.Integration/        # ASP.NET Core, EF Core persistence, SOAP/XML smoke, HL7 TCP/MLLP smoke, restart persistence tests
```

**Structure Decision**: Keep the existing Clean Architecture solution. Do not add a connector bridge project. Add protocol-specific host helpers under Infrastructure and endpoint registration in Api, with Application-owned abstractions and Domain-owned state. Admin UI remains an operator surface for coverage, scenarios, logs, reset, and verification.

## Phase 0 Research Summary

See [research.md](research.md).

Resolved decisions:

- Preserve connector-facing contracts as native-compatible simulator surfaces.
- Keep admin/control APIs under `/api/v1`; keep native connector paths unmodified.
- Model endpoint coverage as data and dispatch through provider-specific handlers.
- Add Application interfaces for dispatch/scenario/auth/log/evidence; implement transports in Api/Infrastructure.
- Persist synthetic state in SQLite until operator reset.
- Enforce synthetic authentication for protected flows.
- Accept documented shapes plus known serializer variants.
- Implement SOAP/WCF/ASMX compatibility by envelope/action fidelity, adding metadata only when connector startup requires it.
- Use a hosted HL7 TCP/MLLP boundary with deterministic ACK/NAK behavior.
- Keep Admin UI as an operator surface, not a connector bridge.

## Phase 1 Design Summary

See [data-model.md](data-model.md), [contracts/external-emr-compatibility-contract.md](contracts/external-emr-compatibility-contract.md), and [quickstart.md](quickstart.md).

Design outputs:

- Persistent endpoint-contract catalog with provider, protocol, path/action, source document, serializer variants, auth requirement, support status, and verification status.
- Provider profiles with native base URL/HL7 settings and synthetic credential sets.
- Persisted synthetic scenario state for patients, reports, device registrations, documents, HL7 messages, request logs, and verification evidence.
- Contract-preservation checklist for Epic, Cerner, Athena/Centricity, and Altera/Allscripts surfaces.
- Quickstart for backend/Admin UI startup and provider smoke workflows.

## Post-Design Constitution Check

| Principle | Status | Notes |
| --- | --- | --- |
| Synthetic Data Only and Offline by Default | Pass | Data model stores synthetic credentials/data only and rejects real secrets in default scenarios |
| Provider Contract Fidelity | Pass | Contract artifact explicitly preserves native paths/actions/framing and rejects connector/bridge rewrites |
| Deterministic Scenario Engine | Pass | Scenario state, reset generation, seeded identifiers, and persisted workflow state support reproducible outcomes |
| Clean Architecture and Explicit Boundaries | Pass | Domain/Application abstractions remain transport-neutral; Api/Infrastructure own host details |
| Observable, Tested, and Versioned Changes | Pass | Logs, coverage evidence, contract tests, integration tests, Swagger/admin docs, and quickstart are planned |

## Complexity Tracking

No constitution violations require justification. Native SOAP/XML and HL7 host components add implementation complexity, but they are required by the clarified connector-compatibility contract and remain isolated behind Application interfaces.
