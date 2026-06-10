# Tasks: EMR Simulator Developer Portal

**Input**: Design documents from `/specs/001-emr-simulator-portal/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are required by the constitution and are included below as first-class tasks for each user story.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **API**: `src/EmrSimulator.Api/`
- **Application**: `src/EmrSimulator.Application/`
- **Domain**: `src/EmrSimulator.Domain/`
- **Infrastructure**: `src/EmrSimulator.Infrastructure/`
- **Admin UI**: `src/EmrSimulator.AdminUi/`
- **Tests**: `tests/EmrSimulator.Tests.Unit/`, `tests/EmrSimulator.Tests.Integration/`, `tests/EmrSimulator.Tests.Contracts/`
- Paths shown below assume the EMR Simulator solution layout - adjust based on plan.md structure

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [X] T001 Create the `src/EmrSimulator.*` and `tests/EmrSimulator.Tests.*` project structure per implementation plan
- [X] T002 Initialize the .NET 8 solution with ASP.NET Core API, Angular admin UI, EF Core, SQLite, and test project dependencies
- [ ] T003 [P] Configure linting, formatting, nullable reference type rules, and shared editor settings

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [X] T004 Set up SQLite persistence, EF Core Fluent API mappings, and initial migrations in `src/EmrSimulator.Infrastructure/` <!-- Closed by T052–T058 (Iteration 2) -->
- [X] T005 [P] Define the base domain entities and shared value objects in `src/EmrSimulator.Domain/`
- [X] T006 [P] Define the shared application contracts and provider abstractions in `src/EmrSimulator.Application/`
- [X] T007 Configure `/api/v1` routing, provider route registration, and startup wiring in `src/EmrSimulator.Api/`
- [X] T008 Implement the deterministic scenario engine and mock response selection pipeline in `src/EmrSimulator.Application/`
- [X] T009 Implement request logging, error handling, and response metadata capture in `src/EmrSimulator.Infrastructure/`
- [X] T010 Add Swagger/OpenAPI setup and environment configuration management in `src/EmrSimulator.Api/`

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Switch Providers and Validate Behavior (Priority: P1) 🎯 MVP

**Goal**: Let users switch between supported providers and get provider-specific simulator behavior on the same synthetic data.

**Independent Test**: Select each supported provider and run a representative lookup or workflow without external EMR access. The response should match the selected provider's contract and behavior.

### Implementation for User Story 1

- [X] T011 [P] [US1] Add provider profile and provider enumeration support in `src/EmrSimulator.Domain/EmrProfile.cs`
- [X] T012 [P] [US1] Add provider-specific route model and contract metadata in `src/EmrSimulator.Application/Providers/`
- [X] T013 [US1] Implement Epic provider request handling in `src/EmrSimulator.Api/Providers/Epic/`
- [X] T014 [US1] Implement Cerner provider request handling in `src/EmrSimulator.Api/Providers/Cerner/`
- [X] T015 [US1] Implement Altera provider request handling in `src/EmrSimulator.Api/Providers/Altera/`
- [X] T016 [US1] Implement Athena Flow provider request handling in `src/EmrSimulator.Api/Providers/AthenaFlow/`
- [X] T017 [US1] Implement Athena Server provider request handling in `src/EmrSimulator.Api/Providers/AthenaServer/`
- [X] T018 [US1] Wire provider switching into the admin UI shell in `src/EmrSimulator.AdminUi/src/app/`
- [X] T019 [US1] Surface provider selection state and active-provider indicators in `src/EmrSimulator.AdminUi/src/app/features/providers/`
- [X] T020 [US1] Connect provider selection to the application layer so provider-specific routes resolve the active profile in `src/EmrSimulator.Application/Providers/`

**Checkpoint**: Provider switching and provider-specific simulation behavior should now work end to end.

### Tests for User Story 1

- [X] T040 [P] [US1] Add contract tests for Epic, Cerner, Altera, Athena Flow, and Athena Server route selection in `tests/EmrSimulator.Tests.Contracts/ProviderRoutesTests.cs`
- [X] T041 [P] [US1] Add integration tests for active-provider switching and lookup behavior in `tests/EmrSimulator.Tests.Integration/ProviderSwitchingTests.cs`

---

## Phase 4: User Story 2 - Simulate Failures and Edge Cases (Priority: P2)

**Goal**: Let users choose deterministic failure scenarios and reproduce the same simulated error behavior on demand.

**Independent Test**: Select a failure scenario and repeat the same request. The simulator should return the same failure condition every time.

### Implementation for User Story 2

- [X] T021 [P] [US2] Add scenario catalog entries for happy path, not found, invalid credentials, unauthorized, timeout, server error, rate limited, and malformed response in `src/EmrSimulator.Domain/Scenario.cs`
- [X] T022 [P] [US2] Implement scenario selection and resolution rules in `src/EmrSimulator.Application/Scenarios/`
- [X] T023 [US2] Apply deterministic failure mapping to provider responses in `src/EmrSimulator.Api/Providers/`
- [X] T024 [US2] Persist scenario state changes and selected scenario metadata in `src/EmrSimulator.Infrastructure/`
- [X] T025 [US2] Add scenario management screens in `src/EmrSimulator.AdminUi/src/app/features/scenarios/`
- [X] T026 [US2] Add scenario selection actions and state synchronization in `src/EmrSimulator.AdminUi/src/app/services/`
- [X] T027 [US2] Ensure request logs record the active scenario for each simulated request in `src/EmrSimulator.Infrastructure/Logging/`

**Checkpoint**: Failure simulation should now be reproducible and selectable without affecting provider behavior outside the chosen scenario.

### Tests for User Story 2

- [X] T042 [P] [US2] Add unit tests for the scenario resolution matrix in `tests/EmrSimulator.Tests.Unit/ScenarioEngineTests.cs`
- [X] T043 [P] [US2] Add integration tests for deterministic failure responses in `tests/EmrSimulator.Tests.Integration/ScenarioFailureTests.cs`

---

## Phase 5: User Story 3 - Manage Synthetic Clinical Data (Priority: P3)

**Goal**: Let users seed, import, and review deterministic synthetic clinical records for reproducible environments.

**Independent Test**: Import valid CSV or JSON patient data and confirm the portal stores the records, rejects invalid rows, and shows a useful import report.

### Implementation for User Story 3

- [X] T028 [P] [US3] Create patient, appointment, order, and result entities in `src/EmrSimulator.Domain/`
- [X] T029 [P] [US3] Implement synthetic data services for patient, appointment, order, and result management in `src/EmrSimulator.Application/Data/`
- [~] T030 [US3] Add persistence mappings and repository support for clinical data in `src/EmrSimulator.Infrastructure/Persistence/` <!-- Superseded — tracked in Iteration 3 Phase 1 T030 with full facade wiring -->
- [X] T031 [US3] Implement CSV patient import parsing and validation in `src/EmrSimulator.Application/Imports/`
- [X] T032 [US3] Implement JSON patient import parsing and validation in `src/EmrSimulator.Application/Imports/`
- [X] T033 [US3] Add duplicate detection and import report generation in `src/EmrSimulator.Application/Imports/`
- [X] T034 [US3] Add synthetic data management screens for patients, appointments, orders, and results in `src/EmrSimulator.AdminUi/src/app/features/data/`
- [X] T035 [US3] Add import wizard UI for CSV and JSON uploads in `src/EmrSimulator.AdminUi/src/app/features/imports/`
- [X] T036 [US3] Wire import actions and data refresh flows between the admin UI and API in `src/EmrSimulator.AdminUi/src/app/services/`
- [X] T037 [US3] Implement appointment route handlers for synthetic clinical data management in `src/EmrSimulator.Api/appointments/`
- [X] T038 [US3] Implement order route handlers for synthetic clinical data management in `src/EmrSimulator.Api/orders/`
- [X] T039 [US3] Implement result route handlers for synthetic clinical data management in `src/EmrSimulator.Api/results/`

**Checkpoint**: Synthetic records can now be created, imported, validated, and reviewed independently of scenario work.

### Tests for User Story 3

- [X] T044 [P] [US3] Add contract tests for appointment, order, and result routes in `tests/EmrSimulator.Tests.Contracts/ClinicalRoutesTests.cs`
- [X] T045 [P] [US3] Add integration tests for CSV import validation and duplicate rejection in `tests/EmrSimulator.Tests.Integration/PatientImportCsvTests.cs`
- [X] T046 [P] [US3] Add integration tests for JSON import validation and import report generation in `tests/EmrSimulator.Tests.Integration/PatientImportJsonTests.cs`

---

## Final Phase: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [ ] T047 [P] Update Swagger examples, route descriptions, and provider summaries in `src/EmrSimulator.Api/` and `src/EmrSimulator.Contracts/`
- [X] T048 [P] Add request log viewer screens and filters in `src/EmrSimulator.AdminUi/src/app/features/request-logs/`
- [ ] T049 Harden validation, error messages, and API response consistency across `src/EmrSimulator.Api/`
- [ ] T050 Refresh quickstart and implementation notes in `specs/001-emr-simulator-portal/quickstart.md` and `specs/001-emr-simulator-portal/plan.md`
- [ ] T051 Verify the final solution against the constitution gates and document any follow-up items in `specs/001-emr-simulator-portal/research.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User stories can then proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2 → P3)
- **Polish (Final Phase)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - May integrate with US1 but should be independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - May integrate with US1/US2 but should be independently testable

### Within Each User Story

- Models and contracts before services
- Services before endpoints and UI wiring
- Core implementation before integration
- Story complete before moving to next priority

### Parallel Opportunities

- All Setup tasks marked [P] can run in parallel
- All Foundational tasks marked [P] can run in parallel (within Phase 2)
- Once Foundational phase completes, all user stories can start in parallel (if team capacity allows)
- Different user stories can be worked on in parallel by different team members
- UI tasks and backend tasks within a story can overlap once the shared contracts for that story are complete

---

## Parallel Example: User Story 1

```bash
Task: "Add provider profile and provider enumeration support in src/EmrSimulator.Domain/EmrProfile.cs"
Task: "Add provider-specific route model and contract metadata in src/EmrSimulator.Application/Providers/"
Task: "Implement Epic provider request handling in src/EmrSimulator.Api/Providers/Epic/"
Task: "Implement Cerner provider request handling in src/EmrSimulator.Api/Providers/Cerner/"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL - blocks all stories)
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Confirm provider switching and provider-specific routes work independently
5. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Validate provider switching → MVP usable
3. Add User Story 2 → Validate deterministic failures → Expand testing value
4. Add User Story 3 → Validate import and data management → Complete synthetic data workflows
5. Finish with polish tasks for shared UX, docs, and request log visibility

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1 provider routes and admin provider switcher
   - Developer B: User Story 2 scenario engine and failure mapping
   - Developer C: User Story 3 import pipeline and synthetic data management
3. Stories complete and integrate independently

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Avoid: vague tasks, same file conflicts, cross-story dependencies that break independence

---

---

# Iteration 2 Tasks: EMR Simulator Developer Portal

**Input**: [plan.md](plan.md) — Iteration 2 | **Date**: 2026-06-10  
**Scope**: SQLite persistence, Angular UI repair, and polish (T003, T004, T030, T047, T049, T050, T051 + new tasks T052–T062)

**Note on carry-over tasks**: T003, T004, T030, T047, T049, T050, T051 were open at the end of Iteration 1. Their checkboxes are maintained in the Iteration 1 section above; the new tasks below expand each into concrete, actionable steps.

---

## Iteration 2 — Phase 1: Setup Completion

**Purpose**: Close the remaining setup task from Iteration 1.

- [ ] T003 Configure linting, formatting, nullable reference type rules, and shared editor settings in `.editorconfig`, `Directory.Build.props`, and `src/EmrSimulator.AdminUi/.eslintrc.json`

---

## Iteration 2 — Phase 2: SQLite Persistence (Foundational)

**Purpose**: Replace in-memory store with EF Core 8 + SQLite so simulator state survives restarts. These tasks MUST complete before T030.

**Independent Test**: Start the API, create a patient via POST `/api/v1/patients`, restart the process, and confirm the patient is still returned by GET `/api/v1/patients`.

- [X] T052 Add `Microsoft.EntityFrameworkCore.Sqlite` and `Microsoft.EntityFrameworkCore.Design` NuGet packages to `src/EmrSimulator.Infrastructure/EmrSimulator.Infrastructure.csproj`
- [X] T053 [P] Create `EmrSimulatorDbContext` with `DbSet` properties for all entities in `src/EmrSimulator.Infrastructure/Persistence/EmrSimulatorDbContext.cs`
- [X] T054 [P] Add `IEntityTypeConfiguration<EmrProfile>` and `IEntityTypeConfiguration<Scenario>` in `src/EmrSimulator.Infrastructure/Persistence/Configurations/EmrProfileConfiguration.cs` and `ScenarioConfiguration.cs`
- [X] T055 [P] Add `IEntityTypeConfiguration<Patient>`, `IEntityTypeConfiguration<Appointment>`, `IEntityTypeConfiguration<Order>`, and `IEntityTypeConfiguration<Result>` in `src/EmrSimulator.Infrastructure/Persistence/Configurations/`
- [X] T056 [P] Add `IEntityTypeConfiguration<MockResponse>` and `IEntityTypeConfiguration<RequestLog>` in `src/EmrSimulator.Infrastructure/Persistence/Configurations/MockResponseConfiguration.cs` and `RequestLogConfiguration.cs`
- [X] T057 Register `EmrSimulatorDbContext` with `UseSqlite` in `src/EmrSimulator.Infrastructure/ServiceCollectionExtensions.cs` and add `"ConnectionStrings": { "Default": "Data Source=emrsimulator.db" }` to `src/EmrSimulator.Api/appsettings.json`
- [X] T058 Add initial EF Core migration named `InitialCreate` in `src/EmrSimulator.Infrastructure/Migrations/` and call `database.EnsureCreated()` in the API startup in `src/EmrSimulator.Api/Program.cs`

**Checkpoint**: `dotnet ef migrations list` shows `InitialCreate`. `dotnet run` starts without migration errors.

---

## Iteration 2 — Phase 3: Clinical Data Persistence (US3 Carry-over)

**Purpose**: Close T030 — wire repository support so clinical entities are stored in SQLite.

- [ ] T030 Add persistence mappings and repository support for clinical data in `src/EmrSimulator.Infrastructure/Persistence/` — implement `IPatientRepository`, `IAppointmentRepository`, `IOrderRepository`, and `IResultRepository` backed by `EmrSimulatorDbContext`
- [ ] T059 Update integration tests in `tests/EmrSimulator.Tests.Integration/` to resolve `EmrSimulatorDbContext` via `services.AddDbContext` with `DataSource=:memory:` and call `EnsureCreated()` in test setup

**Checkpoint**: All existing integration tests pass with the SQLite in-memory provider.

---

## Iteration 2 — Phase 4: Angular UI Repair

**Purpose**: Confirm the admin UI builds and serves correctly from the right working directory.

**Independent Test**: Run `npm run build` from `src/EmrSimulator.AdminUi/` and confirm `dist/emr-simulator-admin-ui/` is produced with no errors. Then run `npm start` and open `http://localhost:4200`.

- [ ] T060 Validate Angular build by running `npm run build` from `src/EmrSimulator.AdminUi/` (NOT from `src/EmrSimulator.AdminUi/src/`) and fix any TypeScript or template compile errors found in `src/EmrSimulator.AdminUi/src/`
- [ ] T061 [P] Confirm `npm start` (`ng serve`) launches without errors and all five nav links (Providers, Scenarios, Data, Imports, Request Logs) render their pages correctly at `http://localhost:4200`

**Checkpoint**: Angular build exits with code 0. Dev server starts at port 4200 without errors.

---

## Iteration 2 — Final Phase: Polish & Constitution Gate

**Purpose**: Close all remaining polish and governance tasks.

- [ ] T047 [P] Update Swagger examples, route descriptions, and provider summaries — add `WithSummary`, `WithDescription`, and `Produces<T>` calls to all route groups in `src/EmrSimulator.Api/`
- [ ] T049 Harden validation, error messages, and API response consistency — return `ValidationProblem` for 400s and `ProblemDetails` for 500s across `src/EmrSimulator.Api/`
- [ ] T050 [P] Mark quickstart and plan as refreshed — verify `specs/001-emr-simulator-portal/quickstart.md` reflects the correct `npm start` path and `specs/001-emr-simulator-portal/plan.md` constitution gate table shows all PASS
- [ ] T062 Add an `EnsureMigrated()` integration test in `tests/EmrSimulator.Tests.Integration/` that verifies the SQLite schema matches the current migration in a file-based test database
- [ ] T051 Verify the final solution against the constitution gates — check all five principles and document any follow-up items in `specs/001-emr-simulator-portal/research.md`

---

## Iteration 2 — Dependencies

```
T052 → T053, T054, T055, T056
T053 + T054 + T055 + T056 → T057
T057 → T058
T058 → T030 → T059
T060 → T061
T047, T049, T050, T060 can run in parallel
T062 → T051
```

## Iteration 2 — Parallel Opportunities

- T053, T054, T055, T056 — all entity configurations, no overlapping files
- T047, T049, T060 — Swagger/API/UI tasks touch different files
- T050, T003 — documentation and config, fully independent

## Iteration 2 — Independent Test Criteria

| Phase | Can be validated independently when… |
|-------|--------------------------------------|
| SQLite (T052–T058) | `dotnet ef migrations list` shows `InitialCreate`; API starts clean |
| Clinical persistence (T030, T059) | All integration tests pass with SQLite in-memory provider |
| Angular UI (T060, T061) | Build exits 0; dev server at port 4200; all five pages render |
| Polish (T047, T049) | Swagger shows summaries for all routes; 400 responses return `ProblemDetails` |
| Constitution gate (T051) | research.md updated with all five gates confirmed PASS |

## Iteration 2 — Suggested MVP

Complete SQLite persistence (T052–T058) first — this closes the only foundational gap. T030 and T059 are tracked in the Iteration 3 section below. Angular UI repair (T060, T061) can proceed in parallel once T003 is done.

---

---

# Iteration 3 Tasks: EMR Simulator Developer Portal

**Input**: [plan.md](plan.md) — Iteration 3 | **Date**: 2026-06-10  
**Scope**: Clinical data repositories, Angular UI validation, and polish/constitution gate  
**Carry-over open tasks**: T003, T030, T047, T049, T050, T059, T060, T061, T062, T063, T051

---

## Iteration 3 — Phase 1: Clinical Data Repositories (US3 Completion)

**Purpose**: Declare repository interfaces in Application layer; back them with EF Core in Infrastructure. Satisfies T030 and enables integration tests to run against SQLite (T059).

**Independent Test**: POST a patient to `/api/v1/patients`, restart the API, GET `/api/v1/patients` — the record must still be present.

- [ ] T064 [P] Add `IPatientRepository` interface in `src/EmrSimulator.Application/Repositories/IPatientRepository.cs` with methods `GetAll()`, `GetById(Guid)`, `Add(Patient)`, `ExistsByMrn(string)`, `ExistsByExternalId(string)`
- [ ] T065 [P] Add `IAppointmentRepository`, `IOrderRepository`, and `IResultRepository` interfaces in `src/EmrSimulator.Application/Repositories/` with `GetAll()` and `GetByPatientId(Guid)` methods; `IResultRepository` also exposes `GetByOrderId(Guid)`
- [ ] T030 Implement `EfPatientRepository`, `EfAppointmentRepository`, `EfOrderRepository`, and `EfResultRepository` in `src/EmrSimulator.Infrastructure/Persistence/` backed by `EmrSimulatorDbContext`; register all four with `AddScoped` in `src/EmrSimulator.Infrastructure/ServiceCollectionExtensions.cs`; update `src/EmrSimulator.Infrastructure/EmrSimulatorFacade.cs` to inject and use `IPatientRepository`, `IAppointmentRepository`, `IOrderRepository`, and `IResultRepository` in place of the direct `InMemoryEmrSimulatorStore` reads for patient, appointment, order, and result data
- [ ] T059 Create `tests/EmrSimulator.Tests.Integration/SimulatorWebApplicationFactory.cs` (or update the existing custom factory if one exists) to override `AddDbContext<EmrSimulatorDbContext>` with `UseSqlite("DataSource=:memory:")` and call `db.Database.EnsureCreated()` in `ConfigureTestServices`; reference the factory in all four existing test classes (`ProviderSwitchingTests`, `ScenarioFailureTests`, `PatientImportCsvTests`, `PatientImportJsonTests`) and confirm all 4 tests still pass

**Checkpoint**: `dotnet test tests/EmrSimulator.Tests.Integration/` passes. POST patient survives API restart.

---

## Iteration 3 — Phase 2: Angular Admin UI Validation

**Purpose**: Confirm the Angular build succeeds from the correct working directory and the dev server starts.

**Independent Test**: `npm run build` exits with code 0 and produces `dist/emr-simulator-admin-ui/`. `npm start` opens `http://localhost:4200` with all five nav links rendering their pages.

- [ ] T060 Run `npm run build` from `src/EmrSimulator.AdminUi/` (where `angular.json` lives); fix any TypeScript or template compile errors in `src/EmrSimulator.AdminUi/src/` until build exits with code 0
- [ ] T061 Run `npm start` from `src/EmrSimulator.AdminUi/`; confirm dev server starts at port 4200 and all five routes — `/providers`, `/scenarios`, `/data`, `/imports`, `/request-logs` — render without console errors

**Checkpoint**: `npm run build` exits 0. `npm start` starts without errors. All five pages visible in browser.

---

## Iteration 3 — Phase 3: Setup Completion

**Purpose**: Close the remaining setup task from Iteration 1.

- [ ] T003 Add `.editorconfig` at the repo root (`C:\Projects\Midmark\src\EmrSimulator\.editorconfig`) with: `indent_size = 4`, `charset = utf-8-bom`, `end_of_line = crlf` for `*.cs`; `indent_size = 2`, `charset = utf-8`, `end_of_line = lf` for `*.ts`, `*.json`, `*.scss`, `*.html`; add `eslint.config.mjs` at `src/EmrSimulator.AdminUi/eslint.config.mjs` using Angular 20 flat-config format (`@angular-eslint/eslint-plugin` + `@angular-eslint/eslint-plugin-template`)

---

## Iteration 3 — Phase 4: Polish (Parallel)

**Purpose**: Swagger enrichment, API hardening, and EF Core configuration verification.

- [ ] T047 [P] Add `WithSummary(...)`, `WithDescription(...)`, and `.Produces<T>(200)` / `.ProducesProblem(400)` to all route groups in `src/EmrSimulator.Api/Program.cs`
- [ ] T049 [P] Add `app.UseExceptionHandler(...)` or a global exception middleware in `src/EmrSimulator.Api/Program.cs` so unhandled exceptions return `ProblemDetails` (500); replace all `Results.BadRequest(new { error = ... })` returns with `Results.ValidationProblem(...)` for consistent 400 shape
- [ ] T063 [P] Add `tests/EmrSimulator.Tests.Unit/Persistence/EntityConfigurationTests.cs` using `EmrSimulatorDbContext` with `UseSqlite("DataSource=:memory:")` to verify: `Patients` table has unique index on `Mrn`; `Scenarios` FK to `EmrProfiles` cascades; `RequestLogs` FK to `Scenarios` is nullable (do NOT use `UseInMemoryDatabase` — it bypasses SQLite constraint enforcement)
- [ ] T062 [P] Add `tests/EmrSimulator.Tests.Integration/PersistenceSchemaTests.cs` that creates a file-based SQLite database, calls `EnsureCreated()`, and asserts the `Patients`, `Scenarios`, `RequestLogs`, `MockResponses`, `Appointments`, `Orders`, and `Results` tables exist
- [ ] T066 [P] Add a timing assertion test in `tests/EmrSimulator.Tests.Integration/PerformanceTests.cs` that calls a provider route endpoint 10 times and asserts the average response duration is under 1000ms, satisfying SC-004

---

## Iteration 3 — Final Phase: Constitution Gate

**Purpose**: Mark all gates PASS and close the feature.

- [ ] T050 [P] Update `specs/001-emr-simulator-portal/plan.md` constitution check table to set all gates to **PASS** once T047, T049, T060 are complete; verify `specs/001-emr-simulator-portal/quickstart.md` needs no further changes
- [ ] T051 Verify all five constitution principles against the delivered implementation and document findings (any follow-up items or confirmed compliance) in `specs/001-emr-simulator-portal/research.md`

---

## Iteration 3 — Dependencies

```
T064 + T065 → T030 → T059
T060 → T061
T047 + T049 + T060 → T050 → T051
T063 → T051
T062 → T051
T003 (independent)
T066 (independent)
```

## Iteration 3 — Parallel Opportunities

- T064, T065 — different files; no shared dependencies
- T047, T049, T063, T062, T003 — all touch different files; safe to run in parallel
- T060, T059 — independent streams (Angular vs. backend tests)
- T050, T051 — only after all other phases complete

## Iteration 3 — Independent Test Criteria

| Phase | Can be validated independently when… |
|-------|--------------------------------------|
| Repositories (T064–T030) | Repositories registered; `dotnet build` succeeds |
| Integration tests (T059) | `dotnet test` Integration project passes all 4 tests |
| Angular build (T060) | `npm run build` exits code 0 |
| Angular serve (T061) | Dev server starts; all 5 pages render |
| EF Core config tests (T063) | Unit test project passes new config assertions |
| Schema test (T062) | Integration schema test creates and validates DB file |
| Constitution gate (T051) | research.md updated; plan.md all gates PASS |

## Iteration 3 — Task Summary

| ID | Story | Description |
|----|-------|-------------|
| T064 | US3 | `IPatientRepository` interface in Application |
| T065 | US3 | `IAppointmentRepository`, `IOrderRepository`, `IResultRepository` interfaces |
| T030 | US3 | EF Core repository implementations + DI registration |
| T059 | US3 | Integration test factory → SQLite in-memory |
| T060 | UI | Angular build validation from correct directory |
| T061 | UI | Angular dev server + five-page smoke test |
| T003 | Setup | `.editorconfig` + `eslint.config.mjs` |
| T047 | Polish | Swagger summaries, descriptions, typed responses |
| T049 | Polish | ProblemDetails middleware + consistent 400 shape |
| T063 | Tests | EF Core configuration unit tests |
| T062 | Tests | SQLite schema migration integration test |
| T050 | Gate | Plan constitution table → all PASS |
| T051 | Gate | Final constitution gate in research.md |
| T066 | Tests | SC-004 performance timing assertion |

**Total new tasks this iteration**: 3 (T064, T065, T066) + 11 carry-over = **14 tasks to close the feature**

