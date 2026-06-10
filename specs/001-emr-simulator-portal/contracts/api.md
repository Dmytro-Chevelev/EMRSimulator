# API Contract: EMR Simulator Developer Portal

## Contract Overview

The simulator exposes a versioned HTTP API under `/api/v1` for provider routes, data management,
scenario control, and request log visibility. Responses must remain synthetic and deterministic
for a given provider and scenario.

## Common Rules

- All routes are versioned under `/api/v1`.
- The simulator must not require access to a live external EMR.
- Request logs must capture request and response metadata for review.
- Swagger/OpenAPI documents the active route surface.

## Provider Route Families

### Epic

- `/api/v1/emr/epic/auth/token`
- `/api/v1/emr/epic/patients/search`
- `/api/v1/emr/epic/patients/{patientId}`

### Cerner

- `/api/v1/emr/cerner/auth/token`
- `/api/v1/emr/cerner/patients/search`
- `/api/v1/emr/cerner/patients/{patientId}`

### Altera

- `/api/v1/emr/altera/auth/token`
- `/api/v1/emr/altera/patients/search`
- `/api/v1/emr/altera/patients/{patientId}`

### Athena Flow

- `/api/v1/emr/athena-flow/auth/token`
- `/api/v1/emr/athena-flow/patients/search`
- `/api/v1/emr/athena-flow/patients/{patientId}`

### Athena Server

- `/api/v1/emr/athena-server/auth/token`
- `/api/v1/emr/athena-server/patients/search`
- `/api/v1/emr/athena-server/patients/{patientId}`

## Admin and Support Routes

- `/api/v1/patients`
- `/api/v1/appointments`
- `/api/v1/appointments/{appointmentId}`
- `/api/v1/orders`
- `/api/v1/orders/{orderId}`
- `/api/v1/results`
- `/api/v1/results/{resultId}`
- `/api/v1/scenarios`
- `/api/v1/request-logs`
- `/api/v1/import/patients`

## Contract Expectations

- Provider routes return provider-shaped payloads for the active scenario.
- Failure scenarios must preserve deterministic status codes and response bodies where documented.
- Import routes must return accepted and rejected records with reasons.
- Log routes must return request history with timing and response details.