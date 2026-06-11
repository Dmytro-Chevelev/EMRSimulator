# Investigation Report Template

Use this structure for the final response. Adapt section length to the complexity of the issue, but keep the headings unless the user asks for a different format.

# Work item investigation: [IDs or short title]

## Executive summary
Provide a concise overview of the issue, the most likely or confirmed root cause, impact, and recommended next action.

## Work items reviewed
| ID | Title | State | Severity/Priority | Area | Key symptom |
|---|---|---|---|---|---|
| [ID] | [Title] | [State] | [Severity/Priority] | [Area] | [Symptom] |

## Artifacts reviewed
List the evidence sources grouped by type:

- **Azure DevOps work items:** [IDs and relationship]
- **Repositories/files:** [repo/path and why reviewed]
- **Commits/PRs:** [IDs, titles, and relevance]
- **Builds/releases/tests:** [IDs and relevance]
- **Attachments/docs/logs:** [names and relevance]

## Symptom and reproduction clues
Summarize the reported behavior, expected behavior, repro steps, affected versions/environments, logs, error messages, and timing clues.

## Code investigation
Explain the relevant code path:

1. Entry point or trigger
2. Main functions/classes/components involved
3. Important conditions/configuration/data dependencies
4. Where actual behavior diverges from expected behavior
5. Relevant tests or missing test coverage

Include file paths, functions/classes, and repository names.

## Root cause assessment
State one of:

- **Confirmed root cause:** evidence directly proves the cause.
- **Most likely root cause:** evidence strongly supports the cause but one or more confirmations are missing.
- **Unconfirmed:** insufficient evidence; provide the strongest hypotheses.

Then explain the reasoning chain from symptom to code behavior.

## Impacted components
List affected services, modules, UI areas, APIs, data models, integrations, environments, or customers if known.

## Recommended fix
Provide specific implementation guidance:

- Files/components to change
- Logic/config/data changes needed
- Edge cases to handle
- Backward compatibility or migration concerns
- Suggested tests to add or update

## Verification plan
Provide concrete validation steps:

1. Unit tests
2. Integration/API tests
3. Manual repro validation
4. Regression checks
5. Deployment/build/release validation, if relevant

## Confidence and unknowns
State confidence as High, Medium, or Low. List missing evidence, unanswered questions, or access limitations.

## Follow-up questions
Ask only the questions needed to unblock the next investigation or implementation step.
