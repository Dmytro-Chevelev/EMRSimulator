# Quickstart: Next Iteration Execution

## Goal

Execute a deterministic verification run for API, tests, and Admin UI from canonical directories, then record explicit gate outcomes.

## Canonical command order

1. From repo root, verify .NET build and tests:

```powershell
cd C:\Projects\Midmark\src\EmrSimulator
dotnet build src/EmrSimulator.Api/EmrSimulator.Api.csproj
dotnet test
```

2. From Admin UI project root (where `angular.json` exists), validate build and serve:

```powershell
cd C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi
npm install
npm run build
npm start
```

## Failure handling and diagnostics workflow

For every failure, record a structured diagnostic entry. Use the helper script for new entries:

```powershell
./scripts/collect-diagnostic.ps1 `
  -Id "D00X-short-name" `
  -Command "npm run build" `
  -Cwd "src/EmrSimulator.AdminUi" `
  -ErrorSignature "Cannot find module 'nanoid/non-secure'" `
  -LikelyCause "nanoid v5 installed; postcss requires v3 subpath export" `
  -NextAction "npm install nanoid@3.3.7 --save-dev --legacy-peer-deps" `
  -Status Open
```

Or add entries manually to `specs/002-next-iteration/verification/diagnostics-log.md` using the same fields.

**Triage checklist (target: under 15 minutes):**

1. Check CWD first — most Angular failures are caused by running from the wrong folder.
2. Compare error signature against existing entries in `diagnostics-log.md`.
3. Confirm all dependencies installed: `node_modules/` exists, `package-lock.json` is present.
4. If `@angular/core` resolution errors appear, delete `node_modules` and run `npm install --legacy-peer-deps` fresh.
5. For `nanoid/non-secure` errors, ensure `nanoid@3.x` is in `devDependencies`, not v5.

**Known-good invocation paths:**

| Command | Required CWD |
|---------|-------------|
| `dotnet build` / `dotnet test` | Repo root (`C:\Projects\Midmark\src\EmrSimulator`) |
| `npm run build` / `npm start` | `src/EmrSimulator.AdminUi` (where `angular.json` lives) |
| `./scripts/verify-local-workflow.ps1` | Repo root |

## Gate closure checklist

- API build pass evidence captured.
- Unit/contracts/integration test evidence captured.
- UI build and serve evidence captured.
- Outstanding blockers marked with remediation and owner.
- Constitution gate statuses updated in planning artifacts.
