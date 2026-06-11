# Tasks: Iteration 3 - Angular UI Resolution and Gate Closure

**Input**: Design documents from `/specs/003-iteration-3/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/api-contract-summary.md, quickstart.md
**Tests**: No TDD tasks are generated because the specification requests command-based build validation and browser smoke verification, not new automated tests.

**Organization**: Tasks are grouped by user story so the Angular Admin UI fix can be delivered as the MVP before gate closure and next-increment documentation.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files or records independent evidence
- **[Story]**: Maps the task to a specific user story from spec.md
- Every task includes an exact repository-relative file path or command working directory path

---

## Phase 1: Setup (Shared Operational Artifacts)

**Purpose**: Prepare the evidence locations and local workflow artifacts needed by all stories.

- [ ] T001 Create the verification directory and iteration evidence tracker at specs/003-iteration-3/verification/iteration-verification.md
- [ ] T002 [P] Create the browser smoke checklist at specs/003-iteration-3/verification/admin-ui-smoke-test.md
- [ ] T003 [P] Create the constitution gate tracker at specs/003-iteration-3/verification/constitution-gates.md
- [ ] T004 [P] Create an Admin UI command guard script at src/EmrSimulator.AdminUi/scripts/verify-admin-ui-root.ps1

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Ensure the Admin UI can be installed and invoked reproducibly before browser verification begins.

**CRITICAL**: Complete this phase before starting any user story verification.

- [ ] T005 Confirm local Angular CLI npm scripts and exact dependency pins in src/EmrSimulator.AdminUi/package.json
- [ ] T006 Wire the Admin UI working-directory guard into npm lifecycle scripts in src/EmrSimulator.AdminUi/package.json
- [ ] T007 Document the canonical Admin UI working directory and wrong-directory recovery in specs/003-iteration-3/quickstart.md
- [ ] T008 Clean stale dependency artifacts in src/EmrSimulator.AdminUi/node_modules and src/EmrSimulator.AdminUi/package-lock.json
- [ ] T009 Run `npm install --legacy-peer-deps` from src/EmrSimulator.AdminUi and record dependency evidence in specs/003-iteration-3/verification/iteration-verification.md

**Checkpoint**: The Admin UI dependency graph is clean and npm commands execute from the correct workspace root.

---

## Phase 3: User Story 1 - Resolve Angular Admin UI build and confirm all five pages work (Priority: P1) MVP

**Goal**: A contributor can build, serve, and browse the Angular Admin UI from src/EmrSimulator.AdminUi without errors.

**Independent Test**: Run `npm install --legacy-peer-deps`, `npm run build`, start the API, run `npm start`, then verify Providers, Scenarios, Data, Imports, and Request Logs render without browser console errors.

### Implementation for User Story 1

- [ ] T010 [US1] Run `npm run build` from src/EmrSimulator.AdminUi and record exit code, build duration, and dist output evidence in specs/003-iteration-3/verification/iteration-verification.md
- [ ] T011 [US1] Validate the wrong-directory guard from src/EmrSimulator.AdminUi/src and record the actionable output in specs/003-iteration-3/verification/iteration-verification.md
- [ ] T012 [US1] Start the API with `dotnet run --project src/EmrSimulator.Api/EmrSimulator.Api.csproj` from the repository root and record the API URL in specs/003-iteration-3/verification/iteration-verification.md
- [ ] T013 [US1] Start the Admin UI with `npm start` from src/EmrSimulator.AdminUi and record 30-second startup plus HTTP 200 evidence in specs/003-iteration-3/verification/iteration-verification.md
- [ ] T014 [US1] Verify http://localhost:4200/providers renders and `/api/v1/providers` returns 200, recording results in specs/003-iteration-3/verification/admin-ui-smoke-test.md
- [ ] T015 [US1] Verify http://localhost:4200/scenarios renders and `/api/v1/scenarios` returns 200, recording results in specs/003-iteration-3/verification/admin-ui-smoke-test.md
- [ ] T016 [US1] Verify http://localhost:4200/data renders and `/api/v1/patients`, `/api/v1/appointments`, `/api/v1/orders`, and `/api/v1/results` return 200, recording results in specs/003-iteration-3/verification/admin-ui-smoke-test.md
- [ ] T017 [US1] Verify http://localhost:4200/imports renders without console errors, recording results in specs/003-iteration-3/verification/admin-ui-smoke-test.md
- [ ] T018 [US1] Verify http://localhost:4200/request-logs renders and `/api/v1/request-logs` returns 200, recording results in specs/003-iteration-3/verification/admin-ui-smoke-test.md
- [ ] T019 [US1] Verify navigation across all five Admin UI links preserves SPA routing and record contract C3-008 results in specs/003-iteration-3/verification/admin-ui-smoke-test.md

**Checkpoint**: User Story 1 is complete when C3-001 through C3-008 are all recorded as passing.

---

## Phase 4: User Story 2 - Close all outstanding iteration quality gates with verified evidence (Priority: P2)

**Goal**: All five constitution gates have explicit pass/fail evidence, with Gate V closed by the Angular build and serve results.

**Independent Test**: Open specs/003-iteration-3/verification/constitution-gates.md and confirm every principle is `Pass` with objective evidence and no unresolved closure-blocking follow-ups.

### Implementation for User Story 2

- [ ] T020 [US2] Run `dotnet test` from the repository root and record the 17-test result in specs/003-iteration-3/verification/iteration-verification.md
- [ ] T021 [US2] Populate Principle I synthetic-data evidence in specs/003-iteration-3/verification/constitution-gates.md
- [ ] T022 [US2] Populate Principle II provider-contract evidence in specs/003-iteration-3/verification/constitution-gates.md
- [ ] T023 [US2] Populate Principle III deterministic-scenario evidence in specs/003-iteration-3/verification/constitution-gates.md
- [ ] T024 [US2] Populate Principle IV Clean Architecture evidence in specs/003-iteration-3/verification/constitution-gates.md
- [ ] T025 [US2] Populate Principle V observable-tested-versioned evidence from Admin UI build, smoke checks, and `dotnet test` in specs/003-iteration-3/verification/constitution-gates.md
- [ ] T026 [US2] Update D006 to Resolved with Angular build evidence in specs/002-next-iteration/verification/diagnostics-log.md
- [ ] T027 [US2] Resolve any non-Pass gate or stop iteration closure, then confirm all gates are `Pass` in specs/003-iteration-3/verification/constitution-gates.md

**Checkpoint**: User Story 2 is complete when all five gate statuses are `Pass` and Gate V has objective evidence.

---

## Phase 5: User Story 3 - Identify and document the next feature increment (Priority: P3)

**Goal**: The next delivery cycle has clear candidate features ready to feed a future `/speckit.specify` run.

**Independent Test**: Open specs/003-iteration-3/research.md and confirm at least two next-increment candidates have names and rationale.

### Implementation for User Story 3

- [ ] T028 [US3] Confirm the Next Increment Candidates table contains at least two candidates with rationale in specs/003-iteration-3/research.md
- [ ] T029 [US3] Add a selected-candidate handoff note or selection criteria for future `/speckit.specify` input in specs/003-iteration-3/research.md
- [ ] T030 [US3] Update the Phase 2 gate closure and next-increment summary in specs/003-iteration-3/plan.md

**Checkpoint**: User Story 3 is complete when planning can begin for the next feature without new discovery.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final review and reproducibility checks across the completed iteration.

- [ ] T031 [P] Validate quickstart commands end-to-end and update any command drift in specs/003-iteration-3/quickstart.md
- [ ] T032 [P] Verify contract coverage C3-001 through C3-008 against specs/003-iteration-3/contracts/api-contract-summary.md
- [ ] T033 Run `/speckit.analyze` for specs/003-iteration-3 and resolve any consistency findings in specs/003-iteration-3/spec.md, specs/003-iteration-3/plan.md, or specs/003-iteration-3/tasks.md
- [ ] T034 Update README Admin UI run instructions if they diverge from specs/003-iteration-3/quickstart.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies; can start immediately.
- **Foundational (Phase 2)**: Depends on Phase 1 because install/build evidence must be recorded in verification files.
- **User Story 1 (Phase 3)**: Depends on Phase 2 because dependency installation and command guards must be ready first.
- **User Story 2 (Phase 4)**: Depends on User Story 1 because Gate V needs Angular build and serve evidence.
- **User Story 3 (Phase 5)**: Can begin after Phase 1, but final handoff should wait until User Story 2 confirms gate status.
- **Polish (Phase 6)**: Depends on all desired user stories being complete.

### User Story Dependencies

- **User Story 1 (P1)**: MVP; no dependency on other user stories after Foundational.
- **User Story 2 (P2)**: Depends on User Story 1 evidence for Gate V closure.
- **User Story 3 (P3)**: Mostly independent documentation work; final wording should reference the closed gate baseline.

### Within Each User Story

- User Story 1: package/install/build evidence before serve evidence; serve evidence before route smoke checks.
- User Story 2: automated test evidence before final Gate V summary; diagnostics log update after build evidence exists.
- User Story 3: candidate confirmation before handoff note and plan summary.

---

## Parallel Opportunities

- T002, T003, and T004 can run in parallel because they create separate artifacts.
- T014 through T018 can be verified independently once the API and Admin UI are running.
- T021 through T024 can be drafted in parallel because each constitution principle has separate evidence.
- T028 and T031 can run in parallel with gate evidence drafting because they touch research.md and quickstart.md respectively.
- T032 can run in parallel with final documentation cleanup after smoke evidence exists.

---

## Parallel Example: User Story 1

```text
Task: "Verify http://localhost:4200/providers renders and `/api/v1/providers` returns 200, recording results in specs/003-iteration-3/verification/admin-ui-smoke-test.md"
Task: "Verify http://localhost:4200/scenarios renders and `/api/v1/scenarios` returns 200, recording results in specs/003-iteration-3/verification/admin-ui-smoke-test.md"
Task: "Verify http://localhost:4200/imports renders without console errors, recording results in specs/003-iteration-3/verification/admin-ui-smoke-test.md"
```

## Parallel Example: User Story 2

```text
Task: "Populate Principle I synthetic-data evidence in specs/003-iteration-3/verification/constitution-gates.md"
Task: "Populate Principle II provider-contract evidence in specs/003-iteration-3/verification/constitution-gates.md"
Task: "Populate Principle IV Clean Architecture evidence in specs/003-iteration-3/verification/constitution-gates.md"
```

## Parallel Example: User Story 3

```text
Task: "Confirm the Next Increment Candidates table contains at least two candidates with rationale in specs/003-iteration-3/research.md"
Task: "Validate quickstart commands end-to-end and update any command drift in specs/003-iteration-3/quickstart.md"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 setup artifacts.
2. Complete Phase 2 dependency and command prerequisites.
3. Complete Phase 3 User Story 1.
4. Stop and validate C3-001 through C3-008 from specs/003-iteration-3/contracts/api-contract-summary.md.

### Incremental Delivery

1. Deliver User Story 1 so the Admin UI is buildable and browsable.
2. Deliver User Story 2 so all constitution gates are objectively closed.
3. Deliver User Story 3 so the next feature increment can start from documented candidates.
4. Complete polish tasks and run `/speckit.analyze` before declaring the feature done.

### Parallel Team Strategy

1. One contributor handles package/install/build tasks in src/EmrSimulator.AdminUi.
2. One contributor prepares verification and gate artifacts under specs/003-iteration-3/verification.
3. One contributor reviews research.md and quickstart.md documentation once the MVP evidence is available.

---

## Notes

- Keep Admin UI commands rooted at src/EmrSimulator.AdminUi where angular.json lives.
- Do not add new Angular routes or components for this iteration.
- Keep nanoid pinned to 3.3.7; upgrading nanoid can reintroduce the `nanoid/non-secure` build failure.
- Stop at each checkpoint and record objective command output or browser evidence before continuing.
