# Diagnostics Log: 002-next-iteration

Generated: 2026-06-10

## Format

Each record per `contracts/verification-contract.md`:
- **ID**: unique key
- **Command**: what was run
- **CWD**: working directory
- **Error Signature**: concise, reproducible error identifier
- **Likely Cause**: root cause hypothesis
- **Next Action**: executable and testable step
- **Status**: `Open | Resolved`

---

## D001 — Angular CLI Not Found

| Field | Value |
|-------|-------|
| ID | `D001-ng-not-found` |
| Command | `npm run build` (script: `ng build`) |
| CWD | `C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi` |
| Error Signature | `'ng' is not recognized as an internal or external command` |
| Likely Cause | `node_modules/.bin/ng` symlink absent because `npm install` had not completed; local Angular CLI bin was not linked. |
| Next Action | Run `npm install --legacy-peer-deps` from `src/EmrSimulator.AdminUi` to restore `node_modules` fully, then re-run `npm run build`. |
| **Status** | **Resolved** |

---

## D002 — npx Cannot Determine Executable

| Field | Value |
|-------|-------|
| ID | `D002-npx-no-executable` |
| Command | `npm run build` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi` |
| Error Signature | `npm error could not determine executable to run` |
| Likely Cause | `@angular/cli` was listed in `devDependencies` but not installed; `npx` can only resolve to local or npx cache — neither had a valid binary. |
| Next Action | Install devDependencies first (`npm install --legacy-peer-deps`), then use `npx ng build` from workspace root. |
| **Status** | **Resolved** |

---

## D003 — Angular Builder Package Not Found

| Field | Value |
|-------|-------|
| ID | `D003-builder-not-found` |
| Command | `npx -y @angular/cli@20 build` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi` |
| Error Signature | `Error: Could not find the '@angular-devkit/build-angular:application' builder's node package.` |
| Likely Cause | `@angular/cli` was invoked from the npx global cache without `@angular-devkit/build-angular` installed locally; the builder package is not bundled with the CLI. |
| Next Action | Install `@angular/cli@20.1.0 @angular-devkit/build-angular@20.1.0 @angular/compiler-cli@20.1.0` as devDependencies and invoke `npx ng build` from the local project. |
| **Status** | **Resolved** |

---

## D004 — nanoid/non-secure Module Not Found

| Field | Value |
|-------|-------|
| ID | `D004-nanoid-missing` |
| Command | `npx ng build` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi` |
| Error Signature | `An unhandled exception occurred: Cannot find module 'nanoid/non-secure'` |
| Likely Cause | `@angular/build` depends on `postcss` which requires `nanoid@3.x` (exports `nanoid/non-secure` subpath); a conflicting resolution installed `nanoid@5.x` which dropped that subpath export. |
| Next Action | `npm install nanoid@^3.3.7 --save-dev --legacy-peer-deps` to pin nanoid at v3; then re-run `npx ng build`. |
| **Status** | **Resolved** |

---

## D005 — ng serve Launched from Wrong Directory (historical)

| Field | Value |
|-------|-------|
| ID | `D005-ng-wrong-cwd` |
| Command | `ng serve` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi\src` |
| Error Signature | Exit code 1; Angular workspace config not found |
| Likely Cause | Terminal was positioned in the TypeScript source subfolder, not the Angular workspace root that contains `angular.json`. |
| Next Action | Always run Angular CLI commands from `src/EmrSimulator.AdminUi` (where `angular.json` lives), never from the nested `src/` subfolder. |
| **Status** | **Resolved** |

---

## D006 — @angular/core Module Resolution Failure After Iterative npm Installs

| Field | Value |
|-------|-------|
| ID | `D006-angular-core-missing` |
| Command | `npx ng build` |
| CWD | `C:\Projects\Midmark\src\EmrSimulator\src\EmrSimulator.AdminUi` |
| Error Signature | `X [ERROR] Could not resolve "@angular/core"` |
| Likely Cause | Three successive `npm install` calls with `--legacy-peer-deps` (first full install, then `@angular/cli@20.1.0`, then `nanoid@^3.3.7`) caused npm to remove 16 packages and change 217 — likely evicting `@angular/core` symlinks from `node_modules/.package-lock.json`. The cascading peer resolution under `--legacy-peer-deps` is particularly fragile when packages are added piecemeal. |
| Next Action | Resolved in Iteration 3 by deleting stale dependency artifacts, running one clean `npm install --legacy-peer-deps`, using exact package pins, and validating `npm run build` exit 0. |
| **Status** | **Resolved** |
