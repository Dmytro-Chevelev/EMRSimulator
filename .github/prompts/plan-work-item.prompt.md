---
name: Plan Workitem Prompt
description: "Use when you need to plan some work for a story, bug, task."
argument-hint: "User story, bug, task to plan"
agent: Planner
---

Prepare the detailed plan for a work item based on the user's description. Follow the planning rules and output rules defined in the Planner agent. If the user's request is ambiguous, ask clarifying questions to narrow down the scope before creating the plan.:

${input:WorkItem:User Story, Bug, Task, PR, or change description}

Before doing any work:
If `WorkItem` is empty, ask the user for it using #tool:vscode/askQuestions.

- if the provided WorkItem is a user story, plan the implementation work needed to complete the story.
- if the provided WorkItem is a bug, plan the investigation and fix. Always include investigation steps to understand the root cause, even if the user seems to know it, to ensure the plan is based on evidence.
- if the provided WorkItem is a task, plan the work needed to complete the task.
- if provided WorkItem don't have enough information use TechnicalWriter agent to left commentary/ask for more details and clarify the requirements before planning.
