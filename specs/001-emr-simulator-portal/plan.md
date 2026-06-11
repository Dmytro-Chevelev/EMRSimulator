# Implementation Plan: EMR Simulator Developer Portal — Iteration 2

**Branch**: `001-emr-simulator-portal` | **Date**: 2026-06-10 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/001-emr-simulator-portal/spec.md`

## Summary

Iteration 1 delivered the .NET 8 backend simulator core (five provider adapters, deterministic
scenario engine, import logic, request logging, Swagger), full xUnit test coverage across unit,
contract, and integration layers, and a manually-scaffolded Angular 20+ admin UI with all five
feature pages wired to the API service.

**Iteration 2 focuses on three remaining themes**:

1. **SQLite persistence** — replace the in-memory store with an EF Core 8 + SQLite implementation
   using Fluent API mappings so simulator state, scenarios, imported data, and request logs survive
   process restarts (T004, T030).
2. **Angular Admin UI repair and validation** — the UI code is correct but `ng serve` was invoked
   from the wrong working directory (`src/` subfolder instead of the project root). Fix the run
   path and confirm a clean build.
3. **Polish** — Swagger example enrichment, API validation hardening, quickstart refresh, and a
   final constitution gate verification (T047, T049, T050, T051).

## Technical Context

**Language/Version**: C# 13 / .NET 8, TypeScript for Angular 20+  
**Primary Dependencies**: ASP.NET Core Web API, EF Core 8, SQLite, Angular 20+, Swagger/OpenAPI, xUnit, FluentAssertions  
**Storage**: SQLite via EF Core 8 with Fluent API mappings (replacing in-memory store)  
**Testing**: xUnit, FluentAssertions; existing test suite passes against in-memory store; SQLite
tests will be validated via integration tests using an in-memory SQLite provider  
**Target Platform**: Local developer workstation and Docker-compatible environments  
**Project Type**: Full-stack web application / local simulator portal  
**Performance Goals**: Average mock responses under 1 second; setup flow under 5 minutes  
**Constraints**: Synthetic data only, no PHI, offline-by-default, deterministic scenario behavior,
`/api/v1` route versioning, Fluent API only (no data annotations per constitution)  
**Scale/Scope**: Five EMR providers, core clinical entities, scenario engine, request logs, import
workflows, Swagger-exposed APIs

## Constitution Check

*Re-checked post-Iteration 1 design. All gates pass.*

| Gate | Status | Notes |
|------|--------|-------|
| Synthetic data only, no PHI | **PASS** | In-memory and planned SQLite store contain only synthetic records |
| Offline/local runtime preserved | **PASS** | No external network calls introduced |
| Provider routes match contracts | **PASS** | Five providers implemented per `contracts/api.md` |
| Scenario behavior is deterministic | **PASS** | Scenario engine drives all provider responses |
| Clean Architecture boundaries intact | **PASS** | `Api → Application ← Infrastructure`, Domain shared |
| Tests, logs, and Swagger accounted for | **PASS** | Full xUnit coverage, request logging, Swagger active |
| EF Core uses Fluent API only | **PENDING** | Iteration 2 must implement mappings without data annotations |
| Angular build produces a deployable artifact | **PENDING** | Build path must be validated this iteration |

## Project Structure

### Documentation (this feature)

```text
specs/001-emr-simulator-portal/
├── plan.md              # This file (Iteration 2)
├── research.md          # Updated with Iteration 2 decisions
├── data-model.md        # Updated with EF Core mapping notes
├── quickstart.md        # Updated with correct run commands
├── contracts/api.md     # Updated with Swagger example guidance
└── tasks.md             # Updated checkboxes; remaining tasks listed below
```

### Source Code

```text
src/
├── EmrSimulator.Api/             # ASP.NET Core host, routes, Swagger
├── EmrSimulator.Application/     # Interfaces, scenario engine, import logic
├── EmrSimulator.Contracts/       # Shared DTOs
├── EmrSimulator.Domain/          # Entities, value objects
├── EmrSimulator.Infrastructure/  # InMemoryStore (→ replace with EF Core DbContext + SQLite)
└── EmrSimulator.AdminUi/         # Angular 20+ standalone app

tests/
├── EmrSimulator.Tests.Unit/        # Scenario engine
├── EmrSimulator.Tests.Contracts/   # Provider and clinical route contracts
└── EmrSimulator.Tests.Integration/ # Provider switching, failure, import
```

**Structure Decision**: No new projects are added this iteration. The SQLite persistence
work lives in `src/EmrSimulator.Infrastructure/` alongside the existing in-memory store.
The in-memory store is retained and used by unit/contract tests; the EF Core DbContext
is used in the production startup path and integration tests configured with the
in-memory SQLite provider (`Microsoft.EntityFrameworkCore.InMemory` or `:memory:` connection string).

## Complexity Tracking

No constitution violations. No complexity exceptions required.

## Remaining Open Tasks

The following tasks from `tasks.md` are not yet complete and define the Iteration 2 scope:

| ID | Description | Phase |
|----|-------------|-------|
| T003 | Configure linting, formatting, nullable reference types, shared editor settings | Setup |
| T004 | Set up SQLite persistence, EF Core Fluent API mappings, and initial migrations in `src/EmrSimulator.Infrastructure/` | Foundational |
| T030 | Add persistence mappings and repository support for clinical data in `src/EmrSimulator.Infrastructure/Persistence/` | US3 |
| T047 | Update Swagger examples, route descriptions, and provider summaries | Polish |
| T049 | Harden validation, error messages, and API response consistency | Polish |
| T050 | Refresh quickstart and implementation notes | Polish |
| T051 | Verify final solution against constitution gates | Polish |

**Angular UI repair** (not a separate task in tasks.md, but blocking the UI):  
`ng serve` must be run from `src/EmrSimulator.AdminUi/`, not from `src/EmrSimulator.AdminUi/src/`.
The UI code compiles correctly when invoked from the project root.

