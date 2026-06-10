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
- Each endpoint must expose a `WithSummary` and `WithDescription` via the minimal API builder.
- Failure scenario responses must include a `X-Simulator-Scenario` response header identifying the active scenario.

## Provider Route Families

### Epic

- `POST /api/v1/emr/epic/auth/token` — Returns a synthetic OAuth token for the Epic surface.
- `GET /api/v1/emr/epic/patients/search?name={name}` — Returns a list of matching synthetic patients in Epic FHIR Bundle format.
- `GET /api/v1/emr/epic/patients/{patientId}` — Returns a single synthetic patient record in Epic FHIR format.

### Cerner

- `POST /api/v1/emr/cerner/auth/token`
- `GET /api/v1/emr/cerner/patients/search?name={name}`
- `GET /api/v1/emr/cerner/patients/{patientId}`

### Altera

- `POST /api/v1/emr/altera/auth/token`
- `GET /api/v1/emr/altera/patients/search?name={name}`
- `GET /api/v1/emr/altera/patients/{patientId}`

### Athena Flow

- `POST /api/v1/emr/athena-flow/auth/token`
- `GET /api/v1/emr/athena-flow/patients/search?name={name}`
- `GET /api/v1/emr/athena-flow/patients/{patientId}`

### Athena Server

- `POST /api/v1/emr/athena-server/auth/token`
- `GET /api/v1/emr/athena-server/patients/search?name={name}`
- `GET /api/v1/emr/athena-server/patients/{patientId}`

## Admin and Support Routes

- `GET  /api/v1/providers` — Returns the list of available providers and the active provider.
- `POST /api/v1/providers/active` — Sets the active provider. Body: `{ "provider": "Epic" }`.
- `GET  /api/v1/patients` — Returns all synthetic patients.
- `GET  /api/v1/patients/{id}` — Returns a single patient.
- `POST /api/v1/patients` — Creates a synthetic patient.
- `GET  /api/v1/appointments` — Returns all appointments.
- `GET  /api/v1/appointments/{appointmentId}` — Returns a single appointment.
- `GET  /api/v1/orders` — Returns all orders.
- `GET  /api/v1/orders/{orderId}` — Returns a single order.
- `GET  /api/v1/results` — Returns all results.
- `GET  /api/v1/results/{resultId}` — Returns a single result.
- `GET  /api/v1/scenarios` — Returns all scenarios and the active scenario.
- `POST /api/v1/scenarios/active` — Sets the active scenario. Body: `{ "scenarioId": "..." }`.
- `GET  /api/v1/request-logs` — Returns recent simulator request logs. Supports `?provider=Epic` filter.
- `POST /api/v1/import/patients` — Accepts multipart/form-data CSV or JSON. Returns an import result with accepted and rejected rows.

## Swagger Expectations (Iteration 2)

Each route group must include:

- `WithSummary("...")` — short phrase shown in the Swagger operation list.
- `WithDescription("...")` — one or two sentences explaining the route behavior.
- `Produces<T>(200)` / `ProducesProblem(400)` — typed response annotations for key status codes.
- Failure scenarios must document the `X-Simulator-Scenario` header in the response description.

## Contract Expectations

- Provider routes return provider-shaped payloads for the active scenario.
- Failure scenarios must preserve deterministic status codes and response bodies where documented.
- Import routes must return accepted and rejected records with reasons.
- Log routes must return request history with timing and response details.


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