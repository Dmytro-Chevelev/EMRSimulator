---
name: Impact Analysis Prompt
description: "Use when you need engineering impact analysis for a story, bug, task, pull request, or branch diff."
argument-hint: "User story, bug, task, PR, or change to analyze"
agent: Impact Analysis Agent
---

Prepare an engineering impact analysis for the following change request:

${input:ChangeRequest:User Story, Bug, Task, PR, or change description}

Before doing any work:
If `ChangeRequest` is empty, ask the user for it using #tool:vscode/askQuestions.

Expectations:

- Inspect the repository before writing the analysis.
- Use current branch changes as evidence when relevant.
- If no comparison target is provided, compare the current branch to `development` when that branch exists.
- Distinguish facts, assumptions, direct impact, indirect impact, risks, test plan, release considerations, and rollback considerations.
- Call out missing information explicitly instead of guessing.
- Do not modify source code unless explicitly requested.
- Generate both outputs from the same content:
  - Markdown file: keep the impact-analysis section structure defined by the agent.
  - DOCX file: apply the Word formatting requirements defined by the agent; do not paste raw Markdown markers into the Word document.

Return the result in the impact-analysis format defined by this agent.
Save the result in a file named `./.github/analysis/<ChangeRequest>-impact-analysis.md` and `./.github/analysis/<ChangeRequest>-impact-analysis.docx`.
