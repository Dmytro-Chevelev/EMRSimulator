# Tasks: Next Iteration Execution

**Input**: Design documents from `/specs/002-next-iteration/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/verification-contract.md, quickstart.md

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create shared verification artifacts used across all stories.

- [X] T001 Create verification workspace directory `specs/002-next-iteration/verification/`
- [X] T002 Create verification tracker template in `specs/002-next-iteration/verification/iteration-verification.md`
- [X] T003 [P] Create diagnostics log template in `specs/002-next-iteration/verification/diagnostics-log.md`
- [X] T004 [P] Create constitution gate tracker in `specs/002-next-iteration/verification/constitution-gates.md`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish reusable command and diagnostics mechanics that all stories depend on.

**CRITICAL**: No user story implementation starts before this phase completes.

- [X] T005 Normalize Admin UI command scripts in `src/EmrSimulator.AdminUi/package.json` to use a single stable invocation strategy
- [X] T006 Add local workflow orchestration script in `scripts/verify-local-workflow.ps1` for API/test/UI command execution order
- [X] T007 [P] Add diagnostics capture helper in `scripts/collect-diagnostic.ps1` aligned to `contracts/verification-contract.md`
- [X] T008 [P] Add usage notes for scripts in `specs/002-next-iteration/quickstart.md`

**Checkpoint**: Foundational command and diagnostics infrastructure is ready.

---

## Phase 3: User Story 1 - Stabilize local delivery workflow (Priority: P1) 🎯 MVP

**Goal**: Ensure contributors can run API, tests, and Admin UI from canonical directories without setup blockers.

**Independent Test**: Execute command sequence from quickstart and verify build/test/serve outcomes are captured in verification records.

- [X] T009 [US1] Fix Angular workspace execution path guidance in `specs/002-next-iteration/quickstart.md` and `README.md`
- [~] T010 [US1] Resolve Angular build prerequisites and version alignment in `src/EmrSimulator.AdminUi/package.json` — **BLOCKED**: pinned to exact 20.1.0 + nanoid 3.3.7; node_modules deleted for clean reinstall
- [X] T011 [US1] Execute `dotnet build` and `dotnet test` from repo root and record outcomes in `specs/002-next-iteration/verification/iteration-verification.md`
- [~] T012 [US1] Execute `npm run build` from `src/EmrSimulator.AdminUi/` and record outcome in `specs/002-next-iteration/verification/iteration-verification.md` — **BLOCKED**: awaiting clean reinstall
- [~] T013 [US1] Execute `npm start` from `src/EmrSimulator.AdminUi/` and record route smoke-check outcome in `specs/002-next-iteration/verification/iteration-verification.md` — **BLOCKED**: depends on T012
- [X] T014 [US1] Record any blocker discovered during command execution in `specs/002-next-iteration/verification/diagnostics-log.md`

**Checkpoint**: Local developer workflow is executable end-to-end with evidence.

---

## Phase 4: User Story 2 - Close iteration quality gates (Priority: P2)

**Goal**: Produce explicit pass/follow-up outcomes for all iteration and constitution gates.

**Independent Test**: Review gate tracker and confirm every required gate has evidence plus pass or follow-up state.

- [X] T015 [US2] Verify API error-shape consistency and Swagger metadata completeness in `src/EmrSimulator.Api/Program.cs`
- [X] T016 [US2] Verify persistence schema/configuration checks in `tests/EmrSimulator.Tests.Unit/Persistence/EntityConfigurationTests.cs` and `tests/EmrSimulator.Tests.Integration/PersistenceSchemaTests.cs`
- [X] T017 [US2] Capture constitution gate evidence in `specs/002-next-iteration/verification/constitution-gates.md`
- [X] T018 [US2] Update iteration status and gate outcomes in `specs/002-next-iteration/plan.md`
- [X] T019 [US2] Update closure decisions and unresolved follow-ups in `specs/002-next-iteration/research.md`

**Checkpoint**: Quality gates are explicitly closed or have tracked follow-up actions.

---

## Phase 5: User Story 3 - Preserve repeatable operational diagnostics (Priority: P3)

**Goal**: Make failure triage fast and repeatable through standardized diagnostic records.

**Independent Test**: Trigger or use known failures and verify diagnostics include cause and executable remediation.

- [X] T020 [US3] Implement required diagnostic fields in `specs/002-next-iteration/verification/diagnostics-log.md` per `specs/002-next-iteration/contracts/verification-contract.md`
- [X] T021 [US3] Add known current failures (`ng serve` wrong cwd and Angular CLI build failure) with root-cause hypotheses in `specs/002-next-iteration/verification/diagnostics-log.md`
- [X] T022 [US3] Add diagnostics-to-remediation workflow section in `specs/002-next-iteration/quickstart.md`
- [X] T023 [US3] Add verification item to diagnostic record traceability mapping in `specs/002-next-iteration/verification/iteration-verification.md`

**Checkpoint**: Diagnostics are standardized, traceable, and actionable.

---

## Phase 6: Polish & Cross-Cutting

**Purpose**: Final consistency checks and publish-ready docs.

- [X] T024 [P] Cross-link all 002 artifacts in `specs/002-next-iteration/plan.md`
- [X] T025 [P] Align project-level run instructions with verified paths in `README.md`
- [X] T026 Execute full quickstart validation and mark final status in `specs/002-next-iteration/verification/iteration-verification.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- Setup (Phase 1): starts immediately
- Foundational (Phase 2): depends on Setup; blocks user stories
- User Story phases (Phase 3-5): depend on Foundational completion
- Polish (Phase 6): depends on completion of desired user stories

### User Story Dependencies

- US1 (P1): starts first after Foundational; establishes command reliability baseline
- US2 (P2): depends on US1 evidence for complete gate closure
- US3 (P3): can start after Foundational; should complete before final polish so diagnostics are reflected in closure records

### Parallel Opportunities

- Phase 1: T003 and T004 can run in parallel after T001
- Phase 2: T007 and T008 can run in parallel after T005/T006 are underway
- US1: T011 and T012 can run in parallel once T010 is complete
- Polish: T024 and T025 can run in parallel

---

## Parallel Example: User Story 1

```bash
# After Angular prerequisites are aligned (T010):
Task: T011 Execute dotnet build/test and capture evidence
Task: T012 Execute npm run build and capture evidence
```

---

## Implementation Strategy

### MVP First (US1 only)

1. Complete Phase 1 (Setup)
2. Complete Phase 2 (Foundational)
3. Complete Phase 3 (US1)
4. Validate end-to-end local workflow before moving on

### Incremental Delivery

1. Deliver US1 command stability and verification evidence
2. Add US2 gate closure evidence and artifact updates
3. Add US3 standardized diagnostics and remediation workflow
4. Finish with Phase 6 polish and final quickstart execution

### Team Parallelization

1. One contributor handles command/tooling normalization (T005-T013)
2. One contributor handles gate evidence and artifact closure (T015-T019)
3. One contributor handles diagnostics standardization and troubleshooting docs (T020-T023)
