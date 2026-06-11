# Feature Specification: Iteration 3 — Angular UI Resolution and Next Feature Increment

**Feature Branch**: `003-iteration-3`  
**Created**: 2026-06-11  
**Status**: Draft  
**Input**: User description: "Iteration 3"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Resolve Angular Admin UI build and confirm all five pages work (Priority: P1)

As a project contributor, I can build and run the Angular Admin UI from the correct directory so I can access all five admin pages in my browser without errors.

**Why this priority**: The Angular build has been blocked since Iteration 2 (D006: npm dependency corruption). Until this is resolved, no one can use or validate the admin portal, which blocks UI-level acceptance of all other features.

**Independent Test**: Can be fully tested by running `npm install --legacy-peer-deps` then `npm run build` from `src/EmrSimulator.AdminUi`, confirming build exits 0, then starting `npm start` and verifying the five routes (Providers, Scenarios, Data, Imports, Request Logs) each load without browser console errors.

**Acceptance Scenarios**:

1. **Given** `node_modules` is absent or freshly cleared, **When** a contributor runs `npm install --legacy-peer-deps` from `src/EmrSimulator.AdminUi`, **Then** all Angular 20.1.0 packages and nanoid@3.3.7 resolve without conflict.
2. **Given** dependencies are installed, **When** `npm run build` is executed from `src/EmrSimulator.AdminUi`, **Then** the build completes with exit code 0 and a `dist/` output folder is created.
3. **Given** a successful build, **When** `npm start` is run from `src/EmrSimulator.AdminUi`, **Then** the dev server starts at `http://localhost:4200` and all five navigation routes render without console errors.
4. **Given** a contributor runs from the wrong directory (`src/EmrSimulator.AdminUi/src`), **When** the Angular CLI command is executed, **Then** an actionable error message or script guard directs them to the correct directory.

---

### User Story 2 - Close all outstanding iteration quality gates with verified evidence (Priority: P2)

As a project maintainer, I can review a complete gate table with objective pass/fail evidence for all constitution principles so I can confidently declare the simulator ready for the next increment.

**Why this priority**: Gate closure converts completed work into a formally verified baseline. Without it, regression risk accumulates and planning decisions rest on unverified assumptions.

**Independent Test**: Can be tested by opening `specs/003-iteration-3/verification/constitution-gates.md` and confirming each of the five constitution principles has explicit evidence and a `Pass` status, with any open follow-ups documented and owned.

**Acceptance Scenarios**:

1. **Given** Iteration 2 left Gate V partially open (Angular build pending), **When** Iteration 3 completes the Angular validation, **Then** Gate V status updates to `Pass` with build and serve evidence recorded.
2. **Given** all five gates are `Pass`, **When** the gate summary table is reviewed, **Then** no gate shows ambiguous status and there are no unresolved follow-ups blocking iteration closure.

---

### User Story 3 - Identify and document the next feature increment (Priority: P3)

As a product owner, I can read a clear description of what comes after Iteration 3 so the team can begin planning the next delivery cycle without ambiguity.

**Why this priority**: Iterative delivery requires a visible backlog horizon. Once operational gates are closed, the team should know what comes next so planning can begin immediately.

**Independent Test**: Can be tested by reading the Iteration 3 research and plan artifacts and confirming a "Next Increment" section names at least two candidate features with brief rationale, enabling `/speckit.specify` to be run for the chosen feature.

**Acceptance Scenarios**:

1. **Given** all current iteration work is closed, **When** the iteration research doc is reviewed, **Then** candidate next features are named with enough context to start a new specification.
2. **Given** a candidate is selected, **When** `/speckit.specify` is run for that feature, **Then** the new spec can be created without requiring additional discovery from scratch.

---

### Edge Cases

- What happens if `npm install --legacy-peer-deps` still produces resolution conflicts after the clean reinstall?
- What happens if `ng serve` starts but one or more of the five routes renders a blank or error page?
- What happens if a constitution gate cannot move to `Pass` because the underlying capability gap requires a new implementation task?
- How are partial gate closures handled — can individual principles pass independently, or must all pass together?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The Angular Admin UI MUST build successfully from `src/EmrSimulator.AdminUi` with a single `npm install --legacy-peer-deps` followed by `npm run build`.
- **FR-002**: The Angular Admin UI MUST serve all five routes — Providers, Scenarios, Data, Imports, Request Logs — without browser console errors when started with `npm start` from `src/EmrSimulator.AdminUi`.
- **FR-003**: Build and serve outcomes MUST be recorded in the iteration verification tracker as objective evidence.
- **FR-004**: All five constitution principle gates MUST reach `Pass` status with evidence before Iteration 3 is declared complete; any non-Pass gate MUST be documented as blocking follow-up work and resolved before closure.
- **FR-005**: The `package.json` in `src/EmrSimulator.AdminUi` MUST pin exact versions for all `@angular/*` packages and `nanoid` to prevent recurrence of D006-class dependency corruption.
- **FR-006**: A "next increment" section MUST be added to the iteration research document naming at least two candidate features with brief rationale.
- **FR-007**: The diagnostics log MUST be updated to mark D006 as Resolved once the Angular build passes.

### Key Entities

- **IterationGateClosure**: A final per-principle pass/fail record with evidence and any open follow-ups.
- **NextIncrementCandidate**: A named feature with a one-sentence rationale, ready to serve as input to `/speckit.specify`.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: `npm run build` exits 0 from `src/EmrSimulator.AdminUi` with no TypeScript or template errors.
- **SC-002**: All five Admin UI pages render in the browser without console errors after `npm start`.
- **SC-003**: All five constitution gates show `Pass` with recorded evidence in `verification/constitution-gates.md`.
- **SC-004**: At least two next-increment candidates are named and documented in the iteration research file.
- **SC-005**: The Angular `package.json` uses exact (not range) version pins so a clean reinstall is reproducible without version drift.

## Assumptions

- The root cause of D006 (nanoid version conflict during incremental npm installs) is already understood and the fix is to delete `node_modules` and run a single fresh `npm install --legacy-peer-deps` with pinned exact versions.
- The .NET backend, API, and all 17 automated tests remain passing from Iteration 2 and require no changes in this iteration.
- No new Angular components or routes are added in this iteration — scope is limited to making the existing Angular application build and serve correctly.
- The Angular application source code itself is correct; build failures are purely dependency resolution issues.
