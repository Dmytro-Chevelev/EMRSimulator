# Software Design Document (SDD)
# EMR Simulator Developer Portal

Version: 1.0
Status: POC / Architecture Baseline

---

# 1. Executive Summary

The EMR Simulator Developer Portal is a local development platform that emulates external EMR systems including:

- Epic
- Cerner
- Altera
- Athena Flow
- Athena Server

The platform enables developers to validate integrations, debug workflows, simulate failures, and test application behavior without access to real EMR environments.

---

# 2. Business Problem

Development teams currently depend on:

- Shared test environments
- Vendor sandboxes
- Limited EMR access
- Unstable integration endpoints

This causes:

- Slow debugging
- Environment contention
- Limited reproducibility
- Difficult CI/CD testing

The simulator provides a fully local and deterministic environment.

---

# 3. Goals

## Functional Goals

- Simulate multiple EMRs
- Support patient workflows
- Support appointment workflows
- Support order workflows
- Support result workflows
- Support configurable scenarios
- Support mock data management

## Non-Functional Goals

- Run locally
- Docker compatible
- Fast startup
- Simple configuration
- Extensible provider model

---

# 4. Architecture

```text
+----------------------+
| Admin UI             |
+----------+-----------+
           |
           v
+----------------------+
| EMR Simulator API    |
+----------+-----------+
           |
           v
+----------------------+
| Scenario Engine      |
+----------+-----------+
           |
           v
+----------------------+
| Provider Layer       |
+----------+-----------+
           |
           v
+----------------------+
| SQLite Database      |
+----------------------+
```

---

# 5. Solution Structure

```text
src/
├─ EmrSimulator.Api
├─ EmrSimulator.Core
├─ EmrSimulator.Infrastructure
├─ EmrSimulator.AdminUi
├─ EmrSimulator.Contracts
└─ EmrSimulator.Tests
```

---

# 6. Technology Stack

Backend:
- .NET 8
- ASP.NET Core

Frontend:
- Angular 20+ (preferred)
- Alternative: Blazor

Database:
- SQLite

ORM:
- EF Core

Documentation:
- Swagger

Testing:
- xUnit
- FluentAssertions

---

# 7. Supported EMRs

## Epic

Base Route

/api/emr/epic

## Cerner

/api/emr/cerner

## Athena Flow

/api/emr/athena-flow

## Athena Server

/api/emr/athena-server

## Altera

/api/emr/altera

---

# 8. Domain Model

## Patient

- Id
- ExternalPatientId
- MRN
- FirstName
- LastName
- DateOfBirth
- Gender
- Phone
- Email

## Appointment

- Id
- PatientId
- StartTime
- EndTime
- ProviderName
- Status

## Order

- Id
- PatientId
- OrderType
- Status

## Result

- Id
- PatientId
- ResultType
- Value

---

# 9. Database Design

## Patients

Stores synthetic patient data.

## Appointments

Stores mock appointments.

## Orders

Stores mock orders.

## Results

Stores mock results.

## Scenarios

Stores behavior configurations.

## RequestLogs

Stores request history.

---

# 10. Scenario Engine

Supported scenarios:

1. Happy Path
2. Patient Not Found
3. Invalid Credentials
4. Unauthorized
5. Timeout
6. Server Error
7. Rate Limited
8. Malformed Response
9. Empty Result Set

---

# 11. API Contracts

## Authentication

POST

/api/emr/{provider}/auth/token

## Patient Search

GET

/api/emr/{provider}/patients/search

## Patient Details

GET

/api/emr/{provider}/patients/{patientId}

## Appointments

GET

/api/emr/{provider}/patients/{patientId}/appointments

## Orders

POST

/api/emr/{provider}/orders

## Results

GET

/api/emr/{provider}/patients/{patientId}/results

---

# 12. Admin APIs

## Patients

GET /api/admin/patients

POST /api/admin/patients

PUT /api/admin/patients/{id}

DELETE /api/admin/patients/{id}

## Import

POST /api/admin/patients/import

## Logs

GET /api/admin/request-logs

---

# 13. Admin UI

## Dashboard

Shows:

- Active provider
- Active scenario
- Request count
- Error count

## Patient Management

Features:

- Create
- Edit
- Delete
- Search
- Import

## Scenario Management

Features:

- Select scenario
- Configure delays
- Configure errors

## Request Log Viewer

Features:

- View payloads
- View responses
- Filter by provider

---

# 14. Import Formats

## JSON

Supported

## CSV

Supported

Maximum size:

10000 records

---

# 15. Security

POC only.

No production authentication required.

Synthetic data only.

No PHI allowed.

---

# 16. Logging

Capture:

- Headers
- Request Body
- Response Body
- Duration
- Status Code

---

# 17. CI/CD

GitHub Actions

Pipeline:

1. Restore
2. Build
3. Test
4. Publish Artifacts

---

# 18. Docker

Containers:

- API
- UI

Optional SQLite volume mount.

---

# 19. Acceptance Criteria

A developer can:

- Run locally
- Select provider
- Import patients
- Search patients
- Create orders
- Retrieve results
- View logs
- Simulate failures

---

# 20. Roadmap

Phase 1
- POC

Phase 2
- FHIR Support
- HL7 Simulation

Phase 3
- Multi-user support
- PostgreSQL

Phase 4
- Cloud deployment

---

# Appendix A

Recommended Repository Name

emr-simulator

# Appendix B

Recommended Solution Name

EmrSimulator

# Appendix C

Recommended Default Port

5050
