<!--
Sync Impact Report
Version change: unversioned -> 1.0.0
Modified principles: template placeholders -> Synthetic Data Only and Offline by Default; template placeholders -> Provider Contract Fidelity; template placeholders -> Deterministic Scenario Engine; template placeholders -> Clean Architecture and Explicit Boundaries; template placeholders -> Observable, Tested, and Versioned Changes
Added sections: Platform and Data Constraints; Delivery Workflow and Quality Gates
Removed sections: placeholder section headings
Templates requiring updates: ✅ updated .specify/templates/plan-template.md; ✅ updated .specify/templates/tasks-template.md
Deferred items: none
-->

# EMR Simulator Developer Portal Constitution

## Core Principles

### I. Synthetic Data Only and Offline by Default
All simulator data MUST be synthetic. No PHI, production exports, or live vendor data may be
stored, transmitted, or required for normal use. The simulator MUST run locally without
dependency on external EMR systems, and any network interaction outside documented import or
test tooling MUST be treated as a defect.

### II. Provider Contract Fidelity
Each provider implementation MUST preserve the documented surface area, response shape,
status codes, and provider-specific quirks for its EMR route. Provider behavior belongs behind
provider adapters, not scattered across shared code. New routes or payload changes MUST map
back to the documented EMR contracts and endpoint inventory.

### III. Deterministic Scenario Engine
All simulated outcomes MUST be driven by explicit scenario state or seeded inputs so the same
request and scenario produce the same response. Required scenarios such as happy path, not
found, invalid credentials, unauthorized, timeout, server error, rate limiting, and malformed
responses MUST remain reproducible for debugging and CI.

### IV. Clean Architecture and Explicit Boundaries
The solution MUST follow the layered `Api -> Application -> Domain -> Infrastructure` rule.
Domain code MUST not depend on infrastructure, transport, UI, or persistence concerns.
Cross-cutting concerns such as persistence, provider integrations, and UI composition MUST
remain isolated behind interfaces and contracts.

### V. Observable, Tested, and Versioned Changes
Behavior changes MUST be covered by tests at the lowest effective layer and must preserve or
update Swagger/OpenAPI, request logging, and scenario documentation as needed. Breaking API or
contract changes MUST be versioned deliberately and accompanied by migration notes or a clear
compatibility story.

## Platform and Data Constraints

The simulator targets .NET 8 with ASP.NET Core, EF Core, and SQLite for the backend.
The admin experience targets Angular 20+ with a modern component model. Database mappings MUST
use EF Core Fluent API, not data annotations. API routes MUST be versioned under `/api/v1`,
Swagger/OpenAPI MUST remain available, and containerization artifacts SHOULD be kept current
when runtime behavior changes.

## Delivery Workflow and Quality Gates

Work should be planned against the active simulator docs before implementation so changes stay
aligned with the contract inventory, architecture decisions, and backlog. Each substantive
change MUST include the narrowest useful validation step, with tests or build checks proving
the updated behavior. New scenarios, providers, or contract surfaces MUST include updated
documentation and request/response examples when applicable.

## Governance

This constitution supersedes local habits and ad hoc guidance when they conflict. Amendments
require a documented rationale, a version bump, and a validation pass against the affected
templates and runtime guidance. Reviews and automation MUST verify compliance with the
principles above, and exceptions MUST be explicitly justified in the change record.

**Version**: 1.0.0 | **Ratified**: 2026-06-10 | **Last Amended**: 2026-06-10
