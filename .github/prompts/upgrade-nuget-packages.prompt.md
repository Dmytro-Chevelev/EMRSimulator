---
agent: agent
description: Audit and upgrade NuGet packages across the repo to their latest stable versions, resolving conflicts and validating the build.
---

Upgrade all NuGet packages in this repository to their latest stable versions. Follow these steps:

## 1. Discover Projects and Package Management Style

Find all `.csproj`, `.fsproj`, and `.vbproj` files in the repository. Before doing anything else, identify which package management style(s) are in use — the upgrade strategy differs for each:

### Central Package Management (Directory.Packages.props)
Check for a `Directory.Packages.props` file at the repo root. If it exists, CPM is enabled:
- All canonical versions live in `Directory.Packages.props` as `<PackageVersion>` entries.
- Project files reference packages with `<PackageReference>` but **no `Version` attribute** (version comes from the central file).
- **Exception:** a project using `VersionOverride="..."` on a `<PackageReference>` is pinned to that specific version — **do not remove or change `VersionOverride` entries without asking.**
- Upgrades are made in `Directory.Packages.props` only, and take effect across all projects at once.

### SDK-Style Projects (modern default)
Projects with `<Project Sdk="Microsoft.NET.Sdk...">` at the top use `<PackageReference>` elements with inline `Version` attributes. This is the expected modern format. Upgrades are made per-project or, if consistent versions are desired, via a shared `Directory.Packages.props` (see above).

### PackageReference with HintPath
Some `<PackageReference>` entries may include a `<HintPath>` pointing to a local copy of the assembly (e.g., from a `packages/` folder or a lib directory). Treat these with caution:
- The local copy may not match a NuGet-hosted package.
- Before upgrading, confirm the package is available on NuGet.org or the configured feed.
- After upgrading, verify the `<HintPath>` is either removed (if NuGet restore now handles the reference) or updated to the new path.

### Legacy packages.config
Check for `packages.config` files alongside project files. These are non-SDK-style projects and require a different upgrade approach:
- `dotnet add package` does **not** work for `packages.config` projects.
- Use `nuget update <packages.config> -Id <PackageName>` or update the version directly in `packages.config` and the corresponding `<HintPath>` in the `.csproj`.
- Note that `packages.config` projects cannot use Central Package Management.
- If the project is otherwise modern, consider recommending migration to `<PackageReference>` style, but do not perform that migration automatically — flag it for the developer.

## 2. Audit Current Package Versions

Based on the style(s) identified above:
- For CPM repos: inspect `Directory.Packages.props` for all `<PackageVersion>` entries. Also scan for any `VersionOverride` usages across all project files.
- For SDK-style projects: collect all `<PackageReference Version="...">` entries per project.
- For `packages.config` projects: read each `packages.config` file for package id and version.

Then run:
```
dotnet list package --outdated
```
to get an authoritative list of packages with newer stable versions available. Cross-reference this with your collected inventory.

Identify:
- Packages with a newer stable version available
- Packages pinned via `VersionOverride` (CPM) or with an explanatory comment — **do not upgrade without asking**
- Packages with `<HintPath>` that may require manual path updates
- Version mismatches between projects (for non-CPM repos)

## 3. Upgrade Packages

Apply upgrades using the approach appropriate for the detected style:

**Central Package Management:**
Edit `Directory.Packages.props`, updating each `<PackageVersion>` to the latest stable version. Do not touch project files unless a `VersionOverride` needs review.

**SDK-style projects:**
```
dotnet add <project> package <PackageName>
```
If multiple projects share a package, upgrade all to the same version.

**packages.config projects:**
Update the `version` attribute in `packages.config` and the corresponding `<HintPath>` in the `.csproj`. Then run `nuget restore` to download the new version.

**All styles:**
- Only upgrade to **stable** (non-prerelease) versions.
- If a major version bump is involved (e.g., 5.x → 6.x), **stop and ask** before proceeding — major upgrades may contain breaking changes.

## 4. Resolve Conflicts

After upgrading, run:
```
dotnet restore
```
Fix any dependency resolution errors or binding redirect issues that appear. For `packages.config` projects, check `app.config` / `web.config` for `<bindingRedirect>` entries that need updating.

## 5. Validate the Build

Run a full build to confirm nothing is broken:
```
dotnet build
```
If the build fails, identify the root cause. Attempt to fix compilation errors caused by the upgrades (e.g., renamed APIs, removed overloads). If a fix is not straightforward, revert that specific package to its previous version and note it in the summary.

## 6. Run Tests

If a test project exists, run the test suite:
```
dotnet test
```
Report any failures introduced by the upgrades.

## 7. Summarize Changes

Provide a markdown summary table of all changes made:

| Package | Previous Version | New Version | Projects / File Affected | Style | Notes |
|---------|-----------------|-------------|--------------------------|-------|-------|

Call out any packages that were skipped (pinned via `VersionOverride`, `HintPath` requiring manual review, major-version bumps pending review, or reverted due to build/test failures). If any `packages.config` projects were found, note them as candidates for migration to `<PackageReference>` style.
