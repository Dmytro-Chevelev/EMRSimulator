# Verification Contract: Next Iteration

## Purpose

Define the required structure for recording iteration verification outcomes so decisions are repeatable and auditable.

## Contract: Verification Item

Required fields:

- `id` (string)
- `title` (string)
- `command` (string)
- `expectedWorkingDirectory` (string)
- `actualOutcome` (`Pass` | `Fail` | `Blocked`)
- `evidence` (string)
- `remediation` (string, required if not `Pass`)

Acceptance rules:

1. A verification item is complete only when `actualOutcome` and `evidence` are present.
2. Any `Fail` or `Blocked` outcome must include a specific remediation step.
3. Verification items must map to at least one functional requirement from `spec.md`.

## Contract: Diagnostic Record

Required fields:

- `command`
- `cwd`
- `errorSignature`
- `likelyCause`
- `nextAction`

Acceptance rules:

1. `errorSignature` must be concise and unique enough to identify the failure class.
2. `nextAction` must be executable and testable.
3. Records must support triage to likely root cause within 15 minutes (SC-004).

## Contract: Gate Result

Required fields:

- `principle`
- `status` (`Pass` | `Needs Follow-up`)
- `evidence`
- `followUp` (required when `Needs Follow-up`)

Acceptance rules:

1. Every constitution principle must have a gate result.
2. `Needs Follow-up` entries must include an explicit owner/action in related planning artifacts.
