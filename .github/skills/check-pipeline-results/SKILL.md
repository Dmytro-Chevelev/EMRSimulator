---
name: check-pipeline-results
description: >
  Use when checking the status or results of an Azure DevOps pipeline run — whether it succeeded,
  failed, or is in progress. Covers debugging failures, understanding what a successful run did,
  reviewing test results, providing pipeline context to the agent, summarizing CI/CD activity,
  or answering questions like "what did the last build do?" or "did the pipeline pass?"
---

# Check Pipeline Results

## Goal

Retrieve the results of an Azure DevOps pipeline run and produce a clear, structured summary
of what happened — regardless of whether the run succeeded or failed. Adapt the depth of
analysis to the outcome: for failures, surface root cause; for successes, summarize what was built,
tested, or deployed.

## Steps

### 1. Identify the Pipeline Run

If the user has not provided a specific run ID or URL, ask for one of the following:
- A direct link to the pipeline run (e.g., `https://dev.azure.com/<org>/<project>/_build/results?buildId=<id>`)
- The pipeline name and branch to look up the most recent run

### 2. Fetch the Pipeline Run Summary

Use the Azure DevOps MCP server to retrieve the pipeline run. Capture:
- Overall result (`succeeded`, `failed`, `partiallySucceeded`, `canceled`)
- Start time, finish time, and duration
- The triggering commit or PR (if available)
- The branch and repository

### 3. Inspect the Step Timeline

Drill into the timeline of the run. Categorize all steps/jobs/stages by result:
- `succeeded` — note any that are worth highlighting (e.g., deployments, test runs)
- `failed` / `partiallySucceeded` — record the step name and any surfaced error message or exit code
- `skipped` / `canceled` — note if relevant to the overall picture

### 4. Retrieve Logs

**For failed steps:** fetch log output and extract the first error or exception, any stack traces, and the lines immediately preceding the failure. Avoid dumping entire logs.

**For succeeded runs (when context is needed):** fetch logs from key steps (e.g., test results, publish, deploy) to summarize what was produced or deployed.

### 5. Summarize Findings

Adapt the summary to the run outcome:

```
## Pipeline Run Summary

- **Result:** <result>
- **Pipeline:** <name>
- **Branch:** <branch>
- **Triggered by:** <commit/PR>
- **Duration:** <duration>

## Steps Overview

| Step | Result | Notes |
|------|--------|-------|
| <name> | <result> | <key detail if any> |

## [If failed] Failures

### <Step Name>
- **Error:** <error message>
- **Log excerpt:**
  <relevant log lines>

## [If failed] Likely Root Cause

<1–3 sentence assessment based on the logs>

## [If failed] Suggested Next Steps

<Actionable items — e.g., fix a test, check a dependency, re-run a flaky step>

## [If succeeded] What Happened

<Summary of what was built, tested, or deployed — useful as context for follow-on agent tasks>
```

## Notes

- If no run is specified, default to the most recent run on the current branch.
- If the pipeline run is still in progress, report current status and any steps that have already completed.
- If logs are unavailable or access is denied, report what is known (step names, result codes) and advise the user to check the run directly.
- Do not speculate on root cause without log evidence. If the logs are ambiguous, say so.
- For succeeded runs, log retrieval is optional — only fetch logs if the agent or user needs to understand what was produced (e.g., artifacts, deployed version, test counts).
