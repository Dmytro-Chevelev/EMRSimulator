# Implementation Plan: EMR Simulator Developer Portal

**Branch**: `001-emr-simulator-portal` | **Date**: 2026-06-10 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/001-emr-simulator-portal/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

Build a local, offline EMR Simulator Developer Portal that emulates Epic, Cerner, Altera,
Athena Flow, and Athena Server with provider-specific routes, deterministic scenario handling,
synthetic clinical data management, request logging, and Swagger-documented APIs on a .NET 8
backend with an Angular admin UI.

## Technical Context

<!--
  ACTION REQUIRED: Replace the content in this section with the technical details
  for the project. The structure here is presented in advisory capacity to guide
  the iteration process.
-->

**Language/Version**: C# 13 / .NET 8, TypeScript for Angular 20+  
**Primary Dependencies**: ASP.NET Core Web API, EF Core 8, SQLite, Angular Material, Swagger/OpenAPI, xUnit, FluentAssertions  
**Storage**: SQLite  
**Testing**: xUnit, FluentAssertions, integration tests for simulator routes and import flows  
**Target Platform**: Local developer workstation and Docker-compatible environments
**Project Type**: Full-stack web application / local simulator portal  
**Performance Goals**: Average mock responses under 1 second; setup flow under 5 minutes  
**Constraints**: Synthetic data only, no PHI, offline-by-default, deterministic scenario behavior, `/api/v1` route versioning  
**Scale/Scope**: Five EMR providers, core clinical entities, scenario engine, request logs, import workflows, Swagger-exposed APIs

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Gates should be derived from the current constitution and must at minimum verify:

- Synthetic data only and no PHI exposure
- Offline/local runtime assumptions are preserved unless explicitly documented otherwise
- Provider routes and payloads match the scanned EMR contracts
- Scenario-driven behavior remains deterministic and reproducible
- Clean Architecture boundaries stay intact
- Required tests, logs, and Swagger updates are accounted for

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)
```text
src/
├── EmrSimulator.Api/
├── EmrSimulator.Application/
├── EmrSimulator.Domain/
├── EmrSimulator.Infrastructure/
├── EmrSimulator.AdminUi/
└── EmrSimulator.Contracts/

tests/
├── EmrSimulator.Tests.Unit/
├── EmrSimulator.Tests.Integration/
└── EmrSimulator.Tests.Contracts/
```

**Structure Decision**: Use a Clean Architecture solution rooted at `src/EmrSimulator.Api`,
`src/EmrSimulator.Application`, `src/EmrSimulator.Domain`, `src/EmrSimulator.Infrastructure`,
`src/EmrSimulator.AdminUi`, and `src/EmrSimulator.Contracts`, with tests in
`tests/EmrSimulator.Tests.Unit`, `tests/EmrSimulator.Tests.Integration`, and
`tests/EmrSimulator.Tests.Contracts`.

## Complexity Tracking

No constitution violations identified. Complexity tracking is not required for this feature.
