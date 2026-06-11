---
name: installshield-installation-engineering
description: installshield and windows installer development support for copilot agents. use when creating, reviewing, debugging, refactoring, documenting, or testing installer work involving installshield projects, .ism files, installscript, basic msi, installscript msi, suite/advanced ui, prerequisites, setup.exe bootstrappers, msi/msm packages, upgrades, patches, custom actions, registry/files/services/com registration, code signing, localization, ci/cd build automation, installer logs, deployment documentation, release readiness, rollback, repair, uninstall, and enterprise installation scenarios.
---

# InstallShield Installation Engineering

## Purpose

Use this skill as the operating guide for Copilot-style agents that help with InstallShield development and installation engineering. Optimize for safe installer changes, reproducible builds, upgrade correctness, supportability, and clear handoffs between development, QA, release, DevOps, and support teams.

## Start Every Task

1. Classify the request as one or more of: authoring, code review, troubleshooting, upgrade/migration, CI/CD, test planning, release readiness, documentation, or support triage.
2. Identify the installer technology before changing anything: Basic MSI, InstallScript, InstallScript MSI, Suite/Advanced UI, merge module, prerequisite, transform, patch, or bootstrapper.
3. Inspect the repository before proposing edits. Look for `.ism`, `.isproj`, `.rul`, `.issuite`, `.prq`, `.msi`, `.mst`, `.msm`, `setup.ini`, signing scripts, build scripts, pipeline files, release notes, and installer logs.
4. State assumptions when artifacts are missing. Continue with safe defaults rather than inventing project-specific GUIDs, paths, product names, certificate details, or build commands.
5. For every proposed change, explain the installer impact: install, upgrade, repair, uninstall, rollback, reboot, elevation, per-user/per-machine behavior, and enterprise deployment impact.

## Non-Negotiable Safety Rules

- Preserve existing component GUIDs for installed resources unless there is a deliberate component-rule reason to change them.
- Do not casually change `ProductCode`, `UpgradeCode`, `PackageCode`, component GUIDs, feature names, key paths, or upgrade table logic.
- Do not delete registry keys, services, files, scheduled tasks, environment variables, or COM registration during upgrade/uninstall unless ownership and conditions are clear.
- Prefer standard MSI/InstallShield capabilities over custom actions. Use custom actions only when built-in tables, views, or project settings cannot satisfy the requirement.
- Do not place secrets, license keys, tokens, certificate passwords, or customer data in project files, logs, command lines, or generated documentation.
- Treat elevated custom actions, service changes, drivers, firewall rules, reboot logic, and system-wide registry writes as high-risk changes requiring explicit review notes.
- Quote paths, avoid writable system locations for temporary execution, validate user-controlled input, and avoid launching executables from insecure directories.
- Keep installer behavior deterministic and silent-install friendly. Do not introduce UI-dependent logic into silent or enterprise deployment flows.

## Authoring Workflow

1. Gather requirements: product name/version, target platforms, install scope, prerequisites, features, install directory, upgrade behavior, reboot policy, signing, localization, and silent deployment requirements.
2. Map payload into MSI concepts: features, components, key paths, destination folders, registry entries, shortcuts, services, COM, file associations, environment variables, and prerequisites.
3. Choose the least complex implementation:
   - Basic MSI for standard MSI behavior.
   - InstallScript only when script-driven setup is truly required.
   - InstallScript MSI only when both MSI transaction support and InstallScript UI/logic are needed.
   - Suite/Advanced UI for chaining multiple packages, prerequisites, or bootstrapper-level orchestration.
4. Define upgrade behavior before implementation. Specify major/minor/small update, versioning, product code behavior, upgrade detection, downgrade blocking, and data preservation.
5. Plan rollback and uninstall at the same time as install. Anything installed should have a clear owner and removal condition.
6. Add or update automated verification: build validation, ICE/MSI validation where available, install/upgrade/uninstall smoke tests, silent install tests, log capture, and artifact signing checks.

## InstallScript and Custom Action Guidance

- Keep InstallScript functions small and purpose-specific. Separate detection, validation, execution, rollback, and logging.
- For MSI custom actions, distinguish immediate, deferred, rollback, and commit actions. Do not make machine-state changes from immediate actions.
- Pass data to deferred MSI custom actions through `CustomActionData` rather than assuming direct property access.
- Schedule elevated/deferred actions only when necessary and document why elevation is required.
- Make custom actions idempotent. They should tolerate repair, rollback, partial install, and re-run scenarios.
- Return clear failures, write actionable log messages, and avoid swallowing exceptions.
- Avoid hard-coded absolute paths, localized folder names, architecture-specific paths, and user-profile assumptions.

## Upgrade, Repair, and Uninstall Rules

- Always analyze upgrade impact before changing component structure, key paths, feature hierarchy, install directories, services, registry entries, or product version logic.
- Preserve user data and configuration unless the requirement explicitly says otherwise.
- Document how the installer detects existing versions and blocks unsupported downgrades.
- Confirm that repair restores missing files/registry/service configuration without overwriting user-owned data.
- Confirm uninstall removes product-owned resources and leaves shared or user-owned resources intact.
- For side-by-side installs, make product identity, install locations, services, shortcuts, and registry keys version-aware.

## Troubleshooting Workflow

1. Ask for or locate the full verbose MSI log, InstallShield setup log, build log, command line, OS version, install scope, user privilege context, and exact failure step.
2. Search logs for the first root-cause failure, not only the final summary. Pay special attention to `Return value 3`, custom action failures, file-in-use messages, privilege errors, prerequisite failures, signature issues, and reboot-required states.
3. Use `scripts/msi_log_triage.py` for large MSI logs when available.
4. Separate build-time issues from install-time issues, bootstrapper issues, prerequisite issues, MSI transaction issues, and application first-run issues.
5. Provide a concise root cause, supporting log lines, likely affected installer area, and minimal corrective action.
6. When uncertain, list the next artifact needed and the exact command or log setting to capture it.

## CI/CD and Release Automation

- Prefer repository-defined build scripts and documented InstallShield automation over ad hoc local steps.
- Keep build configuration explicit: product version, release configuration, media type, signing mode, output paths, prerequisite download/cache strategy, and architecture.
- Ensure build agents have required InstallShield licensing, prerequisites, certificates, timestamp access, and deterministic paths.
- Sign final deliverables and any embedded executables that require signing. Verify signatures after packaging.
- Archive installer outputs, logs, generated manifests, validation reports, and checksums.
- Never expose signing credentials in pipeline logs. Use secure secret stores and masked variables.

## Review Checklist for Pull Requests

Use this checklist in installer-related PR reviews:

- **identity:** product version, product code, package code, upgrade code, upgrade rules, and downgrade behavior are intentional.
- **components:** component GUIDs, key paths, shared resources, 32/64-bit locations, and transitive behavior are correct.
- **payload:** files, registry, shortcuts, services, COM, file associations, prerequisites, and environment variables match requirements.
- **custom actions:** scheduling, elevation, rollback, logging, idempotency, security, and silent behavior are safe.
- **upgrade:** data preservation, old-version detection, remove-existing-products timing, feature migration, and side-by-side behavior are verified.
- **uninstall/repair:** owned resources are removed/restored correctly without damaging shared or user data.
- **enterprise deployment:** silent install/uninstall commands, exit codes, reboot behavior, logging, SCCM/Intune/GPO compatibility, and proxy/offline prerequisite handling are documented.
- **release:** signing, timestamping, checksums, artifact names, release notes, and support diagnostics are complete.

## Output Patterns

For code changes, respond with:

1. **Summary:** what changed and why.
2. **Installer impact:** install, upgrade, repair, uninstall, rollback, reboot, silent deployment.
3. **Files touched:** project files, scripts, prerequisites, pipeline files, docs.
4. **Validation:** commands run or recommended tests.
5. **Risks:** GUID/component/custom action/signing/prerequisite risks that need review.

For troubleshooting, respond with:

1. **Likely root cause.**
2. **Evidence:** relevant log lines or symptoms.
3. **Affected installer area.**
4. **Fix.**
5. **Verification test.**

For new installer requirements, respond with:

1. **Proposed project approach.**
2. **Feature/component/prerequisite model.**
3. **Upgrade/uninstall behavior.**
4. **Build/signing/release plan.**
5. **Test matrix.**

## Reference Files

- Use `references/installshield-playbook.md` for detailed authoring, upgrade, custom action, CI/CD, and support guidance.
- Use `references/review-checklists.md` for PR, release, QA, and support checklists.
- Use `references/troubleshooting.md` for log analysis and failure triage guidance.
- Use `scripts/msi_log_triage.py` to summarize verbose MSI logs and highlight probable root-cause lines.
