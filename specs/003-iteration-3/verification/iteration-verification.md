# Iteration 3 Verification

## Summary

| Check | Status | Evidence |
|-------|--------|----------|
| Dependency install | Pass | `npm install --legacy-peer-deps` completed in about 2 minutes; added 855 packages and audited 856 packages |
| Angular build | Pass | `npm run build` exited 0 in 6.66 seconds; dist index exists |
| Wrong-directory guard | Pass | `npm run build` from `src/EmrSimulator.AdminUi/src` exited 1 with actionable root-directory guidance |
| API startup | Pass | API listening at `http://localhost:5288` |
| Admin UI startup | Pass | `npm start` served `http://localhost:4200/`; bundle generated in 2.954 seconds |
| .NET test suite | Pass | `dotnet test` succeeded: 17 total, 17 succeeded, 0 failed, 0 skipped |
| Consistency analysis | Pass | Final Speckit analysis findings remediated: quickstart URL, Gate V status, spec status, and T033 closure |

## Command Evidence

### Dependency Install

Command: `npm install --legacy-peer-deps` from `src/EmrSimulator.AdminUi`.

Result: Pass. Added 855 packages and audited 856 packages in about 2 minutes.

Pinned package verification:

```text
@angular-devkit/build-angular@20.1.0
@angular/cli@20.1.0
@angular/core@20.1.0
nanoid@3.3.7
```

Note: npm reported 26 audit findings. This does not block Iteration 3 because the acceptance contract is dependency resolution and build/serve viability; dependency remediation should be planned separately.

### Angular Build

Command: `npm run build` from `src/EmrSimulator.AdminUi`.

Result: Pass. Exit code 0. Duration 6.66 seconds, under the 3-minute target.

Output evidence:

```text
Application bundle generation complete. [3.483 seconds]
Output location: C:\Projects\Midmark\src\EMRSimulator\src\EmrSimulator.AdminUi\dist\emr-simulator-admin-ui
```

`dist/emr-simulator-admin-ui/browser/index.html` exists.

### Wrong-Directory Guard

Command: `npm run build` from `src/EmrSimulator.AdminUi/src`.

Result: Pass. The command exits 1 before Angular build execution and reports:

```text
Admin UI commands must be run from src/EmrSimulator.AdminUi, where angular.json is located.
Current directory: C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi\src
```

### API Startup

Command: `dotnet run --project src/EmrSimulator.Api/EmrSimulator.Api.csproj` from repository root.

Result: Pass. API started in Development environment and listened at `http://localhost:5288`.

### Admin UI Startup

Command: `npm start` from `src/EmrSimulator.AdminUi`.

Result: Pass. Dev server started at `http://localhost:4200/` with proxy configuration active.

Startup evidence:

```text
Application bundle generation complete. [2.954 seconds]
Local: http://localhost:4200/
```

### .NET Test Suite

Command: `dotnet test` from repository root.

Result: Pass.

```text
Test summary: total: 17, failed: 0, succeeded: 17, skipped: 0, duration: 9.5s
Build succeeded in 17.6s
```

### Consistency Analysis

Final `/speckit.analyze` pass found documentation closure issues only. Remediated quickstart API URL alignment, plan Gate V status, spec status, and task T033 closure.
