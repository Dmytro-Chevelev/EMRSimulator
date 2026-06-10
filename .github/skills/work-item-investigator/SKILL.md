---
name: work-item-investigator
description: analyze Azure DevOps work items and bugs, inspect related repositories and code through Azure DevOps MCP, determine likely root cause, and produce a detailed issue investigation report. Use when the user asks to investigate, troubleshoot, root-cause, analyze, or summarize one or more ADO work items, bugs, incidents, regressions, defects, or repository-backed product issues. Requires at least one bug/work item ID; if no ID is provided, ask the user for bug IDs using #tool:vscode/askQuestions before proceeding.
---

# Work Item Investigator

Investigate Azure DevOps bugs or work items end-to-end by combining work item analysis, repository search, code review, linked artifact review, and root-cause reasoning.

## Required input

Require at least one Azure DevOps bug/work item ID before starting.

If the user does not provide any bug or work item ID, call:

```text
#tool:vscode/askQuestions
```

Ask for the missing IDs in a concise question such as:

```text
Please provide one or more Azure DevOps bug/work item IDs to investigate.
```

Do not begin repository or code investigation until at least one ID is known.

## Tooling assumptions

Use Azure DevOps MCP as the primary source for:

- Work item details, comments, history, state, tags, area path, iteration, assigned users, links, and attachments.
- Related work items, parent/child links, duplicate links, predecessor/successor links, commits, PRs, branches, builds, releases, and test artifacts.
- Repository discovery and source search.
- Code, configuration, tests, pipelines, scripts, and recent changes across relevant repositories.

Do not assume work items contain file references. Search broadly across likely repositories and code areas.

## Investigation workflow

1. **Normalize the request**
   - Extract all provided bug/work item IDs.
   - Identify any user-provided constraints such as repository names, branch, version, environment, release, customer, error message, component, or date range.
   - If multiple work items are provided, investigate them together and separately; look for shared symptoms, common components, duplicate causes, and causal chains.

2. **Read each work item deeply**
   - Retrieve title, type, state, priority/severity, area path, iteration path, assignee, reporter, created/updated dates, repro steps, expected/actual behavior, acceptance criteria, system info, tags, and custom fields.
   - Read comments and history chronologically.
   - Inspect all links and attachments. Follow related bugs, user stories, tasks, commits, PRs, builds, releases, test cases, wiki/docs, and external links when available.
   - Capture exact error messages, stack traces, endpoint names, UI labels, feature flags, device models, configuration values, and timestamps.

3. **Infer likely technical search targets**
   - From the work item text, derive search terms: feature names, error messages, class names, API routes, UI strings, database tables, config keys, service names, namespaces, telemetry events, build/release names, and customer/environment identifiers.
   - Include synonyms and normalized forms. For example, split camelCase/PascalCase names, try singular/plural, abbreviations, and likely domain terms.
   - Search for each target across repositories before narrowing scope.

4. **Discover relevant repositories and files**
   - Search Azure DevOps repositories for the extracted terms.
   - Prioritize files that define or consume the failing behavior: controllers, handlers, services, domain models, validators, view models, UI components, integration adapters, database migrations, configuration files, tests, and pipeline/deployment scripts.
   - Inspect recent commits and PRs touching those areas, especially near the bug creation date, release date, or reported regression window.
   - If a work item is linked to a commit or PR, follow it first, then expand to neighboring code paths and dependencies.

5. **Review code for root cause**
   - Trace the relevant execution path from user entry point or external event to the failing behavior.
   - Compare expected behavior from the work item to actual code behavior.
   - Look for common defect patterns:
     - missing null/empty handling
     - incorrect condition or branch ordering
     - stale feature flag/config assumptions
     - schema/model mismatch
     - serialization/deserialization mismatch
     - timezone/date conversion errors
     - race conditions or async ordering issues
     - permission/role checks
     - environment-specific configuration
     - API contract drift
     - dependency version changes
     - unhandled error responses
     - test coverage gaps
   - Review tests around the area. Note whether tests exist, whether they cover the reported case, and what test should be added.

6. **Correlate evidence**
   - Tie the observed symptom to specific work item evidence and specific code evidence.
   - Distinguish confirmed facts from hypotheses.
   - When evidence is incomplete, state what is missing and what would confirm or disprove the hypothesis.
   - Prefer precise references: work item IDs, linked PRs/commits, repository paths, function/class names, configuration keys, build/release names, and test names.

7. **Produce the final report**
   - Use the report structure in `references/report-template.md`.
   - Be specific and evidence-based. Avoid vague statements like "likely a code issue" without naming the component and reasoning path.
   - Include confidence level and unknowns.
   - Recommend fixes and verification steps.

## Handling multiple work items

When multiple IDs are provided:

- Start with a table summarizing each work item.
- Identify duplicates, shared components, shared regression windows, and contradictions.
- Provide per-item findings only where causes differ.
- Provide one combined root-cause section if the same underlying defect explains multiple work items.

## Search depth expectations

Search all related items and repositories unless the user limits scope. Because work items often lack file references:

- Search by title keywords, error text, repro step nouns, UI labels, API terms, logs, and domain concepts.
- Search linked work items and PRs for additional terms.
- Search tests and configuration, not only production code.
- Inspect recent changes in the suspected component.
- Stop only when there is enough evidence to explain the issue or when further progress is blocked by missing access/data.

## Output quality rules

- Separate facts, evidence, hypotheses, and recommendations.
- Cite or name every artifact used: work item IDs, repos, file paths, commits, PRs, builds, tests, and docs.
- Include code-level details when available, but avoid dumping large code blocks.
- Note assumptions explicitly.
- If no root cause can be confirmed, provide the strongest hypothesis, confidence, missing evidence, and next diagnostic steps.
- Do not claim a fix is certain unless the evidence supports it.
