# Implementation Plan: Next Iteration Execution

**Branch**: `002-setup-feature-branch` | **Date**: 2026-06-10 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/002-next-iteration/spec.md`

## Summary

This iteration focuses on operational stability and gate closure rather than net-new product surface:
1) make local API/UI/test workflows reproducible from documented directories,
2) close outstanding quality gates with objective pass/fail evidence,
3) standardize diagnostic capture so setup/build failures are triaged quickly.

The technical approach is to consolidate command paths and verification artifacts in docs, keep API behavior/contracts consistent, preserve persistence/schema validation tests, and record blocker outcomes explicitly in iteration documents.

## Technical Context

**Language/Version**: C# 13 / .NET 8 (backend), TypeScript (Angular 20+)  
**Primary Dependencies**: ASP.NET Core minimal APIs, EF Core 8 + SQLite, Swashbuckle/OpenAPI, Angular CLI 20, xUnit  
**Storage**: SQLite (`emrsimulator.db` for local runtime, SQLite memory/file variants for tests)  
**Testing**: xUnit for contracts/unit/integration; workflow verification commands for API and Admin UI  
**Target Platform**: Windows local developer workstation (portable to other local dev platforms)  
**Project Type**: Full-stack local simulator (web API + SPA + test suite)  
**Performance Goals**: Preserve current route responsiveness and keep setup diagnostics actionable within 15 minutes (SC-004)  
**Constraints**: Synthetic data only, offline runtime assumptions, deterministic scenarios, `/api/v1` route versioning, clean architecture boundaries  
**Scale/Scope**: Existing simulator surface only; this iteration closes execution/gate gaps and does not expand provider or domain scope

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Gate | Status | Evidence |
|------|--------|---------|
| Synthetic data only / no PHI | **PASS** | No scope change to data sources; synthetic-only rule retained |
| Offline/local runtime preserved | **PASS** | Local workflow stabilization reinforces this requirement |
| Provider contract fidelity | **PASS** | No new provider routes; Swagger enriched inline only |
| Deterministic scenarios | **PASS** | Existing deterministic scenario model unchanged; ScenarioEngineTests 2/2 |
| Clean Architecture boundaries | **PASS** | Repository interfaces in Application; implementations in Infrastructure |
| Observable/tested/versioned changes | **PASS** | 17 tests pass; ProblemDetails + Swagger wired; diagnostics standardized |

Post-Design Re-check: PASS. See [constitution-gates.md](verification/constitution-gates.md) for per-principle evidence records.

## Project Structure

### Documentation (this feature)

```text
specs/002-next-iteration/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── verification-contract.md
├── verification/              ← new
│   ├── iteration-verification.md
│   ├── diagnostics-log.md
│   └── constitution-gates.md
└── tasks.md
```

**New scripts at repo root:**

```text
scripts/
├── verify-local-workflow.ps1   ← orchestrates full API/test/UI verification run
└── collect-diagnostic.ps1      ← appends structured diagnostic record to diagnostics-log.md
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

**Structure Decision**: Use the existing solution structure; this iteration is process/gate hardening over current components and does not require new projects.

## Phase 0: Research Outcomes

See [research.md](research.md). Key outcomes:
- Clarified canonical command directories and execution order for API/UI/tests.
- Defined blocker tracking model with explicit status/evidence/remediation fields.
- Locked diagnostics format for repeatable triage and handoff.

## Phase 1: Design Outcomes

See [data-model.md](data-model.md), [contracts/verification-contract.md](contracts/verification-contract.md), and [quickstart.md](quickstart.md).

- Data model introduces non-persistent planning entities for verification/gates/diagnostics.
- Contract defines required fields and acceptance semantics for verification evidence.
- Quickstart defines exact run-order and failure-capture procedure for this iteration.

## Phase 2: Implementation Planning Readiness

This feature is ready for `/speckit.tasks` generation. Work should be sequenced as:
1) workflow command-path stabilization,
2) gate evidence capture,
3) diagnostics hardening and closeout updates.

## Complexity Tracking

No constitution violations requiring exceptions.
