# Research: Iteration 3 — Angular UI Resolution and Gate Closure

## Decision 1: Remove `npx` prefix from npm scripts

- **Decision**: Change `npm run` scripts from `npx ng <command>` to `ng <command>`.
- **Rationale**: When npm executes a script it prepends `node_modules/.bin` to `PATH`, so `ng` resolves directly from the local install. Using `npx ng` bypasses this and attempts registry resolution when `node_modules/.bin/ng` is absent, producing "could not determine executable to run" errors that are misleading.
- **Alternatives considered**: Global `@angular/cli` install was rejected because it introduces machine-level dependencies that vary between contributors.

## Decision 2: Single clean install over iterative installs

- **Decision**: Always delete `node_modules` and `package-lock.json` before running `npm install --legacy-peer-deps`, and perform only one install invocation.
- **Rationale**: Three successive installs with `--legacy-peer-deps` each modify the dependency graph incrementally; npm's hoisting logic can evict packages that were already present, corrupting module resolution. A single install from a clean state produces a deterministic graph.
- **Alternatives considered**: Running `npm ci` was considered but `npm ci` requires `package-lock.json` to be committed and up to date, which is not the current state. `npm install` with the clean state is equivalent for this project.

## Decision 3: Pin all version ranges to exact versions

- **Decision**: Replace all remaining `~` and `^` version ranges in `package.json` with exact pinned versions.
- **Rationale**: D006 was triggered by a combination of minor-version changes accumulating through iterative installs. Exact pinning ensures every clean install produces an identical `node_modules` graph, eliminating version-drift failures.
- **Alternatives considered**: Committing `package-lock.json` and using `npm ci` would achieve reproducibility without pins, but requires discipline to keep the lock file current; exact pins in `package.json` are simpler to audit.

## Decision 4: nanoid must stay at 3.3.7

- **Decision**: `nanoid` MUST remain pinned at `3.3.7` and MUST NOT be upgraded to v4 or v5.
- **Rationale**: `@angular/build` bundles `beasties` which imports `postcss` which uses the `nanoid/non-secure` sub-path export. This sub-path was removed in nanoid v4. Any upgrade will break the Angular build with a `Cannot find module 'nanoid/non-secure'` error.
- **Alternatives considered**: Overriding the postcss nanoid resolution via `npm overrides` was considered but adds complexity; pinning is simpler.

## Decision 5: Keep `--legacy-peer-deps` for the Angular 20 dependency graph

- **Decision**: Use `npm install --legacy-peer-deps` for the initial install.
- **Rationale**: Angular 20.1.0 and its tooling have peer dependency declarations that conflict with npm's strict peer resolution. `--legacy-peer-deps` bypasses strict enforcement without altering the installed packages.
- **Alternatives considered**: `--force` was rejected as it can install incompatible packages. Resolving the peer conflicts manually was rejected as too fragile against future CLI updates.

## Decision 6: Angular source code is correct — no component changes needed

- **Decision**: No Angular TypeScript, HTML, or SCSS files are changed in this iteration.
- **Rationale**: All five route components exist and are registered in `app.routes.ts`. The build failures were purely dependency resolution failures, not source errors.
- **Alternatives considered**: Not applicable.

## Next Increment Candidates

The following features are candidates for the next planning cycle, in rough priority order:

| Candidate | Rationale |
|-----------|-----------|
| **CI/CD pipeline** | The project has no automated build or test pipeline. Adding a GitHub Actions or Azure DevOps pipeline would catch regressions before merge and provide a repeatable delivery artifact. This is the highest-value infrastructure investment after the local workflow is stable. |
| **Docker Compose packaging** | A single `docker-compose up` command that starts both the API and the Admin UI would eliminate setup friction for new contributors and provide a deployable local environment. |
| **Admin UI input validation hardening** | The import page accepts arbitrary CSV/JSON without frontend validation. Adding client-side validation with clear error messages would improve contributor experience and reduce invalid import attempts reaching the API. |
| **Live API status indicator in Admin UI** | A connection status badge in the navigation bar that shows whether the API is reachable would make it immediately obvious when the API is not running, reducing a common confusion point. |

## Next Increment Selection Criteria

Prefer the next candidate that best improves repeatable delivery after Iteration 3 closes. If local build, serve, and gate evidence are stable, prioritize **CI/CD pipeline** because it turns the verified local workflow into an automated regression check. If onboarding friction remains the largest blocker, choose **Docker Compose packaging** instead.
