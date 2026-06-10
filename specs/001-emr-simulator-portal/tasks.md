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

- [ ] T004 Set up SQLite persistence, EF Core Fluent API mappings, and initial migrations in `src/EmrSimulator.Infrastructure/`
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
- [ ] T018 [US1] Wire provider switching into the admin UI shell in `src/EmrSimulator.AdminUi/src/app/`
- [ ] T019 [US1] Surface provider selection state and active-provider indicators in `src/EmrSimulator.AdminUi/src/app/features/providers/`
- [ ] T020 [US1] Connect provider selection to the application layer so provider-specific routes resolve the active profile in `src/EmrSimulator.Application/Providers/`

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
- [ ] T025 [US2] Add scenario management screens in `src/EmrSimulator.AdminUi/src/app/features/scenarios/`
- [ ] T026 [US2] Add scenario selection actions and state synchronization in `src/EmrSimulator.AdminUi/src/app/services/`
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
- [ ] T030 [US3] Add persistence mappings and repository support for clinical data in `src/EmrSimulator.Infrastructure/Persistence/`
- [X] T031 [US3] Implement CSV patient import parsing and validation in `src/EmrSimulator.Application/Imports/`
- [X] T032 [US3] Implement JSON patient import parsing and validation in `src/EmrSimulator.Application/Imports/`
- [X] T033 [US3] Add duplicate detection and import report generation in `src/EmrSimulator.Application/Imports/`
- [ ] T034 [US3] Add synthetic data management screens for patients, appointments, orders, and results in `src/EmrSimulator.AdminUi/src/app/features/data/`
- [ ] T035 [US3] Add import wizard UI for CSV and JSON uploads in `src/EmrSimulator.AdminUi/src/app/features/imports/`
- [ ] T036 [US3] Wire import actions and data refresh flows between the admin UI and API in `src/EmrSimulator.AdminUi/src/app/services/`
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
- [ ] T048 [P] Add request log viewer screens and filters in `src/EmrSimulator.AdminUi/src/app/features/request-logs/`
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
