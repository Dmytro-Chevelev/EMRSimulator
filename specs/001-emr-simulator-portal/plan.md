# Implementation Plan: EMR Simulator Developer Portal — Iteration 3

**Branch**: `001-emr-simulator-portal` | **Date**: 2026-06-10 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/001-emr-simulator-portal/spec.md`

## Summary

Iteration 2 delivered the full EF Core 8 + SQLite persistence layer: `EmrSimulatorDbContext`
with 8 entity `DbSet`s, 8 Fluent-API-only `IEntityTypeConfiguration` classes, DI wiring, and
`EnsureCreated` at API startup. All 12 tests continue to pass; build is clean (0 errors).

**Iteration 3 closes the remaining open work in three themes**:

1. **Clinical data repositories** (T030, T059) — implement `IPatientRepository`,
   `IAppointmentRepository`, `IOrderRepository`, and `IResultRepository` declared in
   `src/EmrSimulator.Application/` and backed by `EmrSimulatorDbContext` in Infrastructure.
   Update integration tests to use SQLite in-memory via `WebApplicationFactory`.

2. **Angular Admin UI validation** (T060, T061) — run `npm run build` from the correct
   directory (`src/EmrSimulator.AdminUi/`), fix any compile errors, and confirm `ng serve`
   starts at port 4200 with all five feature pages rendering.

3. **Polish and constitution gate** (T003, T047, T049, T050, T062, T063, T051) — ESLint flat
   config, Swagger enrichment, ProblemDetails hardening, EF Core configuration unit tests,
   schema migration test, and final constitution verification.

## Technical Context

**Language/Version**: C# 13 / .NET 8, TypeScript for Angular 20+  
**Primary Dependencies**: ASP.NET Core Web API, EF Core 8 + SQLite, Angular 20+, Swagger/OpenAPI, xUnit, FluentAssertions  
**Storage**: SQLite (`emrsimulator.db` in production; `DataSource=:memory:` in tests)  
**Testing**: xUnit, FluentAssertions; unit tests use `InMemoryEmrSimulatorStore`; integration tests use `WebApplicationFactory<Program>` with SQLite in-memory service override  
**Target Platform**: Local developer workstation  
**Project Type**: Full-stack web application / local simulator portal  
**Performance Goals**: Average mock responses under 1 second; setup under 5 minutes  
**Constraints**: Synthetic data only, no PHI, offline, deterministic scenarios, `/api/v1` versioning, Fluent API only (no data annotations), repository interfaces in Application layer  
**Scale/Scope**: Five EMR providers, core clinical entities, scenario engine, request logs, import workflows, Swagger-exposed APIs

## Constitution Check

*Re-checked post-Iteration 2. All Fluent-API and data gates now pass.*

| Gate | Status | Notes |
|------|--------|-------|
| Synthetic data only, no PHI | **PASS** | SQLite stores only synthetic records |
| Offline/local runtime preserved | **PASS** | No external network calls; SQLite is file-local |
| Provider routes match contracts | **PASS** | Five providers implemented per `contracts/api.md` |
| Scenario behavior is deterministic | **PASS** | Scenario engine drives all provider responses |
| Clean Architecture boundaries intact | **PASS** | `Api → Application ← Infrastructure`; Domain clean |
| EF Core Fluent API only (no data annotations) | **PASS** | All 8 configurations verified |
| Tests, logs, and Swagger accounted for | **PARTIAL** | 12 tests pass; Swagger registration active; descriptions pending T047 |
| Angular build produces deployable artifact | **PENDING** | T060 must validate build from correct directory |
| Repository interfaces in Application layer | **PENDING** | T030 must declare interfaces in Application per §IV |

## Project Structure

### Documentation (this feature)

```text
specs/001-emr-simulator-portal/
├── plan.md          # This file (Iteration 3)
├── research.md      # Updated with Iteration 3 decisions
├── data-model.md    # Duplicate content cleaned up; EF Core notes retained
├── quickstart.md    # Verified correct — no changes needed
├── contracts/api.md # No changes needed
└── tasks.md         # Iteration 2 T052–T058 complete; Iteration 3 tasks tracked below
```

### Source Code (repository root)

```text
src/
├── EmrSimulator.Api/             # Program.cs: EnsureCreated wired ✓
│                                 # Add: WithSummary/WithDescription/Produces on routes (T047)
│                                 # Add: ProblemDetails middleware (T049)
├── EmrSimulator.Application/     # Add: IPatientRepository, IAppointmentRepository,
│                                 #      IOrderRepository, IResultRepository (T030)
├── EmrSimulator.Contracts/       # No changes this iteration
├── EmrSimulator.Domain/          # MockResponse added ✓; no further changes
├── EmrSimulator.Infrastructure/  # DbContext + 8 Fluent API configurations ✓
│   └── Persistence/              # Add: EfPatientRepository, EfAppointmentRepository,
│                                 #      EfOrderRepository, EfResultRepository (T030)
└── EmrSimulator.AdminUi/         # Validate build from project root (T060, T061)
                                  # Add: eslint.config.mjs (T003)

tests/
├── EmrSimulator.Tests.Unit/      # Add: Persistence/ folder with config tests (T063)
├── EmrSimulator.Tests.Contracts/ # No changes
└── EmrSimulator.Tests.Integration/ # Update: SQLite in-memory via WebApplicationFactory (T059)
                                    # Add: schema migration test (T062)
```

**Structure Decision**: Repository interfaces are declared in `src/EmrSimulator.Application/`
(the boundary) and implemented in `src/EmrSimulator.Infrastructure/Persistence/`. The existing
`IEmrSimulatorFacade` remains the single public API facade. No new projects are added.

## Complexity Tracking

No constitution violations. No complexity exceptions required.

## Remaining Open Tasks (Iteration 3 Scope)

| ID | Description | Priority |
|----|-------------|----------|
| T064 | Add `IPatientRepository` interface in `src/EmrSimulator.Application/Repositories/` | High |
| T065 | Add `IAppointmentRepository`, `IOrderRepository`, `IResultRepository` interfaces in `src/EmrSimulator.Application/Repositories/` | High |
| T030 | Implement EF Core repository classes; wire facade to use repositories; register in DI | High |
| T059 | Update integration tests: SQLite in-memory override in `WebApplicationFactory` | High |
| T060 | Run `npm run build` from `src/EmrSimulator.AdminUi/`; fix any compile errors | High |
| T061 | Confirm `npm start` starts at port 4200; all five pages render | High |
| T003 | Add `eslint.config.mjs` and `.editorconfig` | Medium |
| T047 | Add `WithSummary`, `WithDescription`, `Produces<T>` to all route groups | Medium |
| T049 | Return `ValidationProblem` for 400s and `ProblemDetails` for 500s | Medium |
| T063 | Add EF Core configuration unit tests in `tests/EmrSimulator.Tests.Unit/Persistence/` | Medium |
| T062 | Add schema migration integration test | Medium |
| T050 | Verify quickstart.md; mark plan constitution gate table all PASS | Low |
| T051 | Final constitution gate — document in `research.md` | Gate |

**Note on T004**: Closed. The foundational SQLite setup task is satisfied by T052–T058 (completed in Iteration 2).

