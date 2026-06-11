# Iteration Verification Tracker: 002-next-iteration

Generated: 2026-06-10

## Format

Each item includes:
- **ID**: unique key (e.g. `T011-dotnet-build`)
- **Title**: short description
- **Category**: `Build | Serve | Test | Gate | Docs`
- **Command**: exact command that was run
- **CWD**: directory command was executed from
- **Expected Outcome**: what success looks like
- **Actual Outcome**: `Pass | Fail | Blocked`
- **Evidence**: command output summary (trimmed)
- **Remediation**: empty if Pass, otherwise next step

---

## T011 — .NET Build

| Field | Value |
|-------|-------|
| ID | `T011-dotnet-build` |
| Category | Build |
| Command | `dotnet build src/EmrSimulator.Api/EmrSimulator.Api.csproj --no-incremental -v minimal` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator` |
| Expected Outcome | Build succeeded. 0 Warning(s). 0 Error(s). |
| **Actual Outcome** | **Pass** |
| Evidence | `Build succeeded. 0 Warning(s). 0 Error(s).` |
| Remediation | — |

---

## T011 — .NET Tests

| Field | Value |
|-------|-------|
| ID | `T011-dotnet-test` |
| Category | Test |
| Command | `dotnet test -v q` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator` |
| Expected Outcome | All test projects pass: Contracts, Unit, Integration |
| **Actual Outcome** | **Pass** |
| Evidence | `Passed! Contracts: 6/6 · Unit: 5/5 · Integration: 6/6` |
| Remediation | — |

---

## T010 — Angular Dependency Alignment

| Field | Value |
|-------|-------|
| ID | `T010-ng-deps` |
| Category | Build |
| Command | `npm install --legacy-peer-deps` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi` |
| Expected Outcome | All `@angular/*` packages at 20.1.0, `nanoid@3.3.7` present, no resolution conflicts |
| **Actual Outcome** | **Blocked** |
| Evidence | Iterative installs corrupted resolution: `@angular/core` became unresolvable; clean reinstall required. `package.json` pinned to exact `20.1.0` versions with `nanoid: 3.3.7`. Node modules deleted. |
| Remediation | Run `npm install --legacy-peer-deps` from `src/EmrSimulator.AdminUi` on a fresh `node_modules` (deletion in progress). |

| Field | Value |
|-------|-------|
| ID | `T012-ng-build` |
| Category | Build |
| Command | `npx ng build` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi` |
| Expected Outcome | Build exits 0, dist output created |
| **Actual Outcome** | **Blocked** |
| Evidence | Cannot complete until `node_modules` clean reinstall finishes. Root cause: `nanoid@3` vs `nanoid@5` conflict corrupted `@angular/core` module resolution across three iterative `npm install` invocations. Fix: pinned exact versions in `package.json`, deleted `node_modules`, reinstall required. |
| Remediation | From `src/EmrSimulator.AdminUi`: `npm install --legacy-peer-deps`, then `npx ng build` |

---

## T013 — Angular Serve

| Field | Value |
|-------|-------|
| ID | `T013-ng-serve` |
| Category | Serve |
| Command | `npx ng serve` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi` |
| Expected Outcome | Dev server starts at `http://localhost:4200` without errors |
| **Actual Outcome** | **Blocked** |
| Evidence | Blocked pending T012 build success |
| Remediation | Complete T012 first |

---

## T015 — API Error Shape & Swagger

| Field | Value |
|-------|-------|
| ID | `T015-api-swagger` |
| Category | Gate |
| Command | Inspect `src/EmrSimulator.Api/Program.cs` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator` |
| Expected Outcome | All routes have `WithSummary`, `WithDescription`, `Produces<T>`; 400s return `ValidationProblem`; 500s return `ProblemDetails` |
| **Actual Outcome** | **Pass** |
| Evidence | `Program.cs` verified — `AddProblemDetails()` registered, `UseExceptionHandler()` wired, all routes decorated with `WithSummary`/`WithDescription`/`Produces<T>`, 400s use `Results.ValidationProblem`, provider errors use `Results.Problem` |
| Remediation | — |

---

## T016 — Persistence Schema Tests

| Field | Value |
|-------|-------|
| ID | `T016-persistence-tests` |
| Category | Test |
| Command | `dotnet test tests/EmrSimulator.Tests.Unit/EmrSimulator.Tests.Unit.csproj -v q` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator` |
| Expected Outcome | EntityConfigurationTests pass: unique Mrn index, cascade FK, nullable ScenarioId |
| **Actual Outcome** | **Pass** |
| Evidence | `Passed! — Unit: 5/5 (including 3 EntityConfigurationTests)` |
| Remediation | — |

---

## T016 — Schema Integration Tests

| Field | Value |
|-------|-------|
| ID | `T016-schema-integration` |
| Category | Test |
| Command | `dotnet test tests/EmrSimulator.Tests.Integration/EmrSimulator.Tests.Integration.csproj -v q` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator` |
| Expected Outcome | PersistenceSchemaTests pass; all 7 tables verified |
| **Actual Outcome** | **Pass** |
| Evidence | `Passed! — Integration: 6/6 (including PersistenceSchemaTests and PerformanceTests)` |
| Remediation | — |
