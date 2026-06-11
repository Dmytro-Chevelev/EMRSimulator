# Data Model: Next Iteration Execution

This iteration introduces planning-level entities used for verification and closure tracking. These are documentation entities for process control and do not require runtime database schema changes.

## Entity: IterationVerificationItem

- id: string (unique key, e.g., `T060-build`)
- title: string
- category: enum (`Build`, `Serve`, `Test`, `Gate`, `Docs`)
- command: string
- expectedWorkingDirectory: string
- expectedOutcome: string
- actualOutcome: enum (`Pass`, `Fail`, `Blocked`)
- evidence: string (command output summary or link/reference)
- remediation: string (empty if pass)
- owner: string
- completedAtUtc: datetime?

Validation rules:
- `actualOutcome=Pass` requires non-empty `evidence`.
- `actualOutcome=Fail|Blocked` requires non-empty `remediation`.
- `expectedWorkingDirectory` must be an existing repository path.

## Entity: WorkflowDiagnosticRecord

- id: string
- command: string
- cwd: string
- errorSignature: string
- likelyCause: string
- nextAction: string
- status: enum (`Open`, `Resolved`)
- createdAtUtc: datetime
- resolvedAtUtc: datetime?

Validation rules:
- `errorSignature` must be concise and reproducible.
- `status=Resolved` requires `resolvedAtUtc`.

## Entity: ConstitutionGateResult

- principle: string
- status: enum (`Pass`, `Needs Follow-up`)
- evidence: string
- followUp: string?

Validation rules:
- `status=Pass` requires evidence.
- `status=Needs Follow-up` requires evidence and `followUp`.

## Relationships

- One IterationVerificationItem can emit zero or more WorkflowDiagnosticRecords.
- IterationVerificationItems collectively provide evidence for ConstitutionGateResults.
