---
applyTo: '**'
---

# Azure DevOps Information

Organization: `Midmark`
Project: `Diagnostics Engineering`

# MCP Server

Use the `az-devops` MCP server configured in `.vscode/mcp.json` for Azure DevOps operations in this repository. Do not refer to the server as `ado` in prompts or instructions.

The configured `az-devops` server exposes these Azure DevOps areas:

- Work items
- Repositories
- Pull requests
- Pipelines
- Artifacts

# Preferred tools

Use Azure DevOps MCP tools before falling back to manual REST shapes or generic terminal/web workflows.

Prefer to use batch updates when updating multiple work items. Use the `wit_update_work_items_batch` tool instead of multiple calls to `wit_update_work_item`.

When retrieving multiple work items, prefer to use the `wit_get_work_items_batch_by_ids` tool instead of multiple calls to `wit_get_work_item`.

Whenever adding tasks to a work item, ensure the area and iteration of the task match the work item.

When a request involves Azure DevOps, identify the correct scope first:

- Work item lookup, planning, comments, state changes, and hierarchy: use `az-devops` work item tools
- Branches, commits, repositories, and PR metadata: use `az-devops` repository and pull request tools
- Build or release run status, logs, and timelines: use `az-devops` pipeline tools
- Published packages or pipeline artifacts: use `az-devops` artifact tools

# General interaction rules

- Confirm the target work item, pull request, repository, pipeline, or run before making changes when the target is ambiguous.
- Prefer reading current state first, then updating only the specific fields or properties required.
- For significant external updates, present the planned change to the user before writing to Azure DevOps unless the user clearly asked for direct execution.
- Keep summaries concise and evidence-based; do not speculate about pipeline failures or work item intent when the current state can be retrieved.
- Preserve existing titles, descriptions, tags, and relations unless the requested change requires modifying them.

# Work item workflow

For work item requests, use this sequence unless the user asks for a narrower action:

1. Retrieve the target work item and any relevant parent or child items.
2. Inspect state, assigned user, area path, iteration path, acceptance criteria, description, relations, and comments as needed.
3. If multiple related work items must be read or updated, switch to the batch tools.
4. When creating child tasks, copy the parent work item's area and iteration unless the user explicitly asks otherwise.
5. When updating fields, patch only the requested fields and preserve unrelated data.

Use comments for progress notes or review notes when the user asks to log work without rewriting the work item description.

# Pull request and repository workflow

- Use `az-devops` repository and pull request tools for PR discovery, metadata, and updates.
- If the user asks for a PR description, first identify the repo and existing PR for the current branch when possible.
- Update an existing PR when one exists; do not create a new PR unless the user explicitly asks.
- Keep PR titles and descriptions tied to the related work item or story when that relationship is known.

# Pipeline workflow

- If a run ID or URL is available, use it directly.
- If not, ask for the pipeline name and branch, or default to the latest relevant run when the user's request implies that behavior.
- Summarize overall result, duration, triggering commit or PR, branch, failed jobs or steps, and the first actionable error from logs.
- Avoid dumping full logs; include only the relevant excerpt needed to explain the outcome.
- If a run succeeded, summarize what was built, tested, published, or deployed when that context matters.

# Artifact workflow

- Use artifact tools when the user asks about published packages, build artifacts, or downloadable outputs.
- Confirm which pipeline run or package version the user is referring to before retrieving artifact details.

# Multiline Field Format Preference

**CRITICAL: Always use Markdown format for all multiline fields (Description, Acceptance Criteria, Repro Steps, custom large text fields).**

When updating work items via REST API, you MUST include TWO operations: one to set the field value and one to set the format. The `wit_update_work_item` and `wit_update_work_items_batch` tools require this two-operation approach:

```json
{
  "updates": [
    {
      "op": "add",
      "path": "/fields/System.Description",
      "value": "# Task Description\n\nMarkdown formatted content..."
    },
    {
      "op": "add",
      "path": "/multilineFieldsFormat/System.Description",
      "value": "Markdown"
    }
  ]
}
```

**Key Points:**

- The default format is HTML if not specified
- Always include the `/multilineFieldsFormat/{FieldName}` operation when updating Description, Acceptance Criteria, Repro Steps, or other large text fields
- Use `"op": "add"` for the format operation (even when updating existing work items)
- Once a field is set to Markdown, it cannot be reverted to HTML
- This applies to all large text fields including custom fields (e.g., `Microsoft.VSTS.Common.AcceptanceCriteria`)
- Use Markdown headings, lists, checkboxes, code fences, and links instead of HTML tags in work item content

**Example for Acceptance Criteria:**

```json
{
  "updates": [
    {
      "op": "add",
      "path": "/fields/Microsoft.VSTS.Common.AcceptanceCriteria",
      "value": "## Acceptance Criteria\n\n- [ ] Criterion 1\n- [ ] Criterion 2"
    },
    {
      "op": "add",
      "path": "/multilineFieldsFormat/Microsoft.VSTS.Common.AcceptanceCriteria",
      "value": "Markdown"
    }
  ]
}
```

# Practical defaults

- Default organization: `Midmark`
- Default project: `Diagnostics Engineering`
- Default Azure DevOps MCP server name in this repo: `az-devops`
