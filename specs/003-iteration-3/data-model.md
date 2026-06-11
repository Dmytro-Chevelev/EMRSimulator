# Data Model: Iteration 3

This iteration introduces no new runtime data entities. The changes are confined to:

1. `package.json` (build tooling configuration)
2. `src/EmrSimulator.AdminUi/scripts/verify-admin-ui-root.ps1` (command guard)
3. Iteration verification and gate closure artifacts (planning documents only)

## Planning Entities (documentation only, not persisted)

### IterationGateClosure

Recorded in `specs/003-iteration-3/verification/constitution-gates.md`.

| Field | Type | Description |
|-------|------|-------------|
| principle | string | Constitution principle name (I–V) |
| status | `Pass` \| `Blocked` | Final outcome; `Blocked` prevents iteration closure |
| evidence | string | Objective proof (command output, test results, screenshot reference) |
| followUp | string? | Required when status is `Blocked`; must be resolved before closure |

### NextIncrementCandidate

Recorded in `specs/003-iteration-3/research.md` under "Next Increment Candidates".

| Field | Type | Description |
|-------|------|-------------|
| name | string | Feature name, concise |
| rationale | string | One-sentence business justification |

## No schema migrations required

The SQLite database schema, EF Core entities, and domain model are unchanged. All 17 existing automated tests continue to pass without modification.
