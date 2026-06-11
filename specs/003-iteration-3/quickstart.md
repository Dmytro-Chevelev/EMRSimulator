# Quickstart: Iteration 3 — Running the Admin UI

## Prerequisites

- Node.js 20+ and npm 10+ installed
- .NET 8 SDK installed
- Repository cloned to `C:\Projects\Midmark\src\EmrSimulator`

---

## Step 1: Start the API

Open a terminal and run from the **repository root**:

```powershell
cd C:\Projects\Midmark\src\EmrSimulator
dotnet run --project src/EmrSimulator.Api/EmrSimulator.Api.csproj
```

Leave this terminal running. The default development profile starts the API at `http://localhost:5288`.

If you override the API URL, update `src/EmrSimulator.AdminUi/proxy.conf.json` so the Angular dev server forwards `/api` to the same backend.

---

## Step 2: Install Admin UI dependencies

Open a **second terminal** and run from the **Admin UI project root**:

```powershell
cd C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi
```

> **Important**: All `npm` commands MUST be run from this directory (where `angular.json` lives).  
> Running from `src/EmrSimulator.AdminUi/src` or the repo root will fail with an actionable guard message that points back to this directory.

If `node_modules` is absent or you need a clean install:

```powershell
# Remove any corrupted state first:
Remove-Item -Recurse -Force node_modules, package-lock.json -ErrorAction SilentlyContinue

# Single clean install:
npm install --legacy-peer-deps
```

Expected output ends with: `added NNN packages, and audited NNN packages`

---

## Step 3: Build (optional validation)

```powershell
npm run build
```

Expected: Exits 0. `dist/emr-simulator-admin-ui/` folder created.

---

## Step 4: Start the Admin UI dev server

```powershell
npm start
```

Expected: `Application bundle generation complete. [X.XXX seconds]` then  
`Watch mode enabled. Watching for file changes...`

The dev server proxies `/api` requests to `http://localhost:5288`, so keep the API running from Step 1 while browsing the Admin UI.

Open a browser and navigate to `http://localhost:4200`.

---

## Step 5: Verify all five pages

Navigate to each page and confirm it renders without browser console errors:

| Page | URL | What to check |
|------|-----|---------------|
| Providers | `http://localhost:4200/providers` | Provider list loads; active provider shown |
| Scenarios | `http://localhost:4200/scenarios` | Scenario list loads |
| Data | `http://localhost:4200/data` | Patient/appointment/order/result data loads |
| Imports | `http://localhost:4200/imports` | Import form renders |
| Request Logs | `http://localhost:4200/request-logs` | Log table loads |

Open Browser DevTools (F12) → Console tab → confirm zero errors on each page.

---

## Troubleshooting

| Symptom | Likely Cause | Fix |
|---------|-------------|-----|
| `ng is not recognized` | Running `ng` directly outside npm script | Use `npm run build` / `npm start`, not `ng build` / `ng serve` |
| `Admin UI commands must be run from src/EmrSimulator.AdminUi` | Running an npm script from the wrong directory | Change to `C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi` and rerun the command |
| `could not determine executable to run` | `node_modules` missing | Run `npm install --legacy-peer-deps` first |
| `Cannot find module '@angular/core'` | Corrupted incremental install | Delete `node_modules` + `package-lock.json`, reinstall |
| `Cannot find module 'nanoid/non-secure'` | nanoid upgraded past v3 | Check `package.json` — `nanoid` must be `"3.3.7"` exactly |
| Page shows blank or error | API not running | Ensure `dotnet run` is running in a separate terminal |
| Wrong port | API port mismatch | Check `src/EmrSimulator.AdminUi/src/app/core/` for API base URL config |

---

## Run all .NET tests

```powershell
cd C:\Projects\Midmark\src\EmrSimulator
dotnet test
```

Expected: 17 tests pass (6 Contracts, 5 Unit, 6 Integration).
