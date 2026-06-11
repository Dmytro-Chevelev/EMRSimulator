# Implementation Plan: Iteration 3 - Angular UI Resolution and Gate Closure

**Branch**: `003-iteration-3` | **Date**: 2026-06-11 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/003-iteration-3/spec.md`

## Summary

Iteration 3 has one primary technical goal and two supporting goals:

1. **Make the Angular Admin UI build and run.** After this iteration the application is fully
  functional end-to-end: API, database, and Admin UI all work together locally. A contributor
  must be able to open a browser and interact with all five admin pages.
2. **Close all open quality gates** with objective pass evidence so the project baseline is
  verified before the next feature increment begins. Any gate that cannot pass blocks closure
  and must become follow-up work before the iteration is declared complete.
3. **Document next increment candidates** so planning for the following cycle can start
  immediately.

## Technical Context

**Language/Version**: TypeScript / Angular 20.1.0 (UI); C# 13 / .NET 8 (API unchanged)  
**Primary Dependencies**: Angular CLI 20.1.0, `@angular-devkit/build-angular` 20.1.0, nanoid 3.3.7, Angular 20.1.0 runtime packages  
**Storage**: N/A (UI iteration; no new data storage changes)  
**Testing**: Angular build exit code; browser navigation smoke test across all five routes; existing xUnit suite (17 tests) must stay green  
**Target Platform**: Windows local developer workstation (`src/EmrSimulator.AdminUi` as the working directory for all UI commands)  
**Project Type**: SPA fix and operational gate closure  
**Performance Goals**: Build completes in under 3 minutes; dev server starts in under 30 seconds  
**Constraints**: No new Angular components or routes; exact npm version pins for all packages; npm scripts MUST work via `npm run <command>` only; `node_modules/.bin` is the canonical CLI path; wrong-directory execution must produce actionable guidance  
**Scale/Scope**: Five existing routes (Providers, Scenarios, Data, Imports, Request Logs); no backend source changes

## Constitution Check

*Pre-implementation gate. Re-checked after Phase 1.*

| Principle | Status | Notes |
|-----------|--------|-------|
| I - Synthetic data / no PHI | **Pass** | No data model changes; UI only reads existing synthetic data |
| II - Provider contract fidelity | **Pass** | No new provider routes; Admin UI calls existing `/api/v1` endpoints |
| III - Deterministic scenarios | **Pass** | No scenario engine changes; UI exposes existing scenario state |
| IV - Clean Architecture boundaries | **Pass** | Angular SPA communicates only through the API surface; no boundary changes |
| V - Observable, tested, versioned changes | **IN PROGRESS** | Angular build and serve evidence will be captured; 17 existing tests must remain green |

Post-Phase-1 re-check target: all five gates at `Pass` with evidence recorded. A non-Pass gate is not an accepted final state for Iteration 3; it must be converted into follow-up work and resolved before closure.

## Root Cause Analysis: D006 Angular Build Failure

The Angular build failure had two compounding causes:

**Cause 1 - Wrong CLI invocation in npm scripts**: Scripts used `npx ng <command>`. When
`node_modules` is absent, `npx` cannot resolve `ng` locally and tries to fetch it from the
npm registry, but the package name `ng` does not exist on the registry, producing a
confusing "could not determine executable to run" error. The fix is to remove the `npx`
prefix. When invoked via `npm run <command>`, npm automatically adds `node_modules/.bin` to
`PATH`, so `ng build` resolves correctly from the local install.

**Cause 2 - Iterative partial installs corrupted the dependency graph**: Three successive
`npm install` calls with different package lists caused npm to add and remove packages in
ways that broke `@angular/core` module resolution. The fix is a single clean install from a
fresh `node_modules` using the pinned `package.json`.

**nanoid note**: `@angular/build` bundles `beasties` which requires `postcss` which requires
`nanoid/non-secure`, a sub-path export only available in nanoid v3. The current
`package.json` pins `nanoid: "3.3.7"`. This is correct and must not be changed.

## Project Structure

### Documentation (this feature)

```text
specs/003-iteration-3/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── api-contract-summary.md
├── verification/
│   ├── admin-ui-smoke-test.md
│   ├── constitution-gates.md
│   └── iteration-verification.md
└── tasks.md
```

### Source Code Changes

```text
src/EmrSimulator.AdminUi/
├── package.json
└── scripts/
   └── verify-admin-ui-root.ps1
```

Planned source changes are intentionally narrow:

- `src/EmrSimulator.AdminUi/package.json`: remove `npx` prefixes from npm scripts, pin remaining ranged versions, and wire the working-directory guard into npm lifecycle scripts.
- `src/EmrSimulator.AdminUi/scripts/verify-admin-ui-root.ps1`: fail fast with actionable guidance when commands are run from a directory that does not contain `angular.json`.

All Angular routes, components, .NET source, tests, and infrastructure files are unchanged.

**Structure Decision**: Keep the existing solution and Angular project structure. The only runtime-adjacent work is package/tooling configuration plus a small guard script; the rest of the iteration produces verification evidence.

## Phase 0: Research Outcomes

See [research.md](research.md). Key findings:

- `npx ng` in npm scripts is unreliable when `node_modules` may be absent; use `ng` directly because npm run adds `.bin` to `PATH`.
- Iterative installs with `--legacy-peer-deps` are fragile; always do a single clean install.
- nanoid@3.3.7 is the correct pin; nanoid v4+ breaks the `nanoid/non-secure` sub-path.
- `package.json` ranges on `rxjs`, `tslib`, `zone.js`, and `typescript` should be pinned to prevent future drift.
- Next increment candidates: CI/CD pipeline; Docker Compose packaging; Admin UI input validation hardening; live API status indicator.

## Phase 1: Design and Fix

### package.json changes

**Scripts**: Remove `npx` prefix and invoke local Angular CLI through npm, for example `ng serve`, `ng build`, and `ng test`.

**Guard integration**: Add npm lifecycle hooks or script composition that runs `scripts/verify-admin-ui-root.ps1` before build, start, and test commands so wrong-directory execution produces a clear correction.

**Pin remaining ranges**:

| Package | Previous range | Pinned to |
|---------|----------------|-----------|
| `rxjs` | `~7.8.0` | `7.8.1` |
| `tslib` | `^2.8.0` | `2.8.1` |
| `zone.js` | `~0.15.0` | `0.15.0` |
| `typescript` | `~5.8.2` | `5.8.2` |

### Install and build sequence

```powershell
# Step 1 - from src/EmrSimulator.AdminUi:
Remove-Item -Recurse -Force node_modules, package-lock.json -ErrorAction SilentlyContinue

# Step 2 - single clean install:
npm install --legacy-peer-deps

# Step 3 - build:
npm run build
# Expected: exits 0, dist/emr-simulator-admin-ui/ created

# Step 4 - serve:
npm start
# Expected: dev server at http://localhost:4200
```

### Verification checkpoints

| Check | Command | Expected |
|-------|---------|----------|
| Install resolves | `npm install --legacy-peer-deps` | Exit code 0, Angular 20.1.0 and nanoid 3.3.7 present |
| Build exits 0 | `npm run build` | Exit code 0, duration under 3 minutes recorded, `dist/` folder present |
| Wrong-directory guard | Run guarded npm command from `src/EmrSimulator.AdminUi/src` | Actionable message points to `src/EmrSimulator.AdminUi` |
| Dev server starts | `npm start` | HTTP 200 at `http://localhost:4200` within 30 seconds |
| Providers page | Navigate to `/providers` | Page renders, API call succeeds |
| Scenarios page | Navigate to `/scenarios` | Page renders, API call succeeds |
| Data page | Navigate to `/data` | Page renders, API calls succeed |
| Imports page | Navigate to `/imports` | Page renders |
| Request Logs page | Navigate to `/request-logs` | Page renders, API call succeeds |
| No console errors | Browser DevTools | Zero errors in Console tab across all five pages |

## Phase 2: Gate Closure and Next Increment

After the Angular build and serve checks pass:

1. Update `specs/002-next-iteration/verification/diagnostics-log.md` and mark D006 Resolved.
2. Create `specs/003-iteration-3/verification/constitution-gates.md` with all five gates at `Pass`.
3. Update `specs/003-iteration-3/verification/iteration-verification.md` with install, build, serve, wrong-directory guard, and `dotnet test` evidence.
4. Update `specs/003-iteration-3/verification/admin-ui-smoke-test.md` with C3-001 through C3-008 results.
5. Keep `specs/003-iteration-3/research.md` ready for the next planning cycle with named candidate features and selection criteria.

## Complexity Tracking

No constitution violations. No new complexity introduced. This remains a dependency and operational closure iteration with a small command guard to satisfy the wrong-directory acceptance scenario.
