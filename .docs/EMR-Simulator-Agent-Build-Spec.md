# AI Agent Build Specification
# EMR Simulator Developer Portal

Version: 1.0
Purpose: Master specification for AI coding agents (Spec, Speck, Copilot, Cursor, Claude Code, ChatGPT Agents)

---

# Mission

Build a production-quality POC for a local EMR Simulator platform that emulates:

- Epic
- Cerner
- Altera
- Athena Flow
- Athena Server

The system must allow developers to test and debug EMR integrations without external environments.

---

# Technology Standards

## Backend

- .NET 8
- ASP.NET Core Web API
- C# 13
- Nullable Reference Types enabled
- Async/Await everywhere

## Frontend

- Angular 20+
- Standalone Components
- Signals preferred
- Angular Material

## Database

- SQLite (POC)
- EF Core 8

---

# Architecture Requirements

Use Clean Architecture.

```text
src/
├─ EmrSimulator.Api
├─ EmrSimulator.Application
├─ EmrSimulator.Domain
├─ EmrSimulator.Infrastructure
├─ EmrSimulator.AdminUi
└─ EmrSimulator.Tests
```

Dependency Rule:

Domain
↑
Application
↑
Infrastructure
↑
Api

No inward dependency violations.

---

# Domain Objects

Required:

- Patient
- Appointment
- Order
- Result
- EmrProfile
- Scenario
- MockResponse
- RequestLog

---

# Coding Standards

## C#

- File-scoped namespaces
- Primary constructors when appropriate
- Records for DTOs
- Strong typing
- No magic strings

## Naming

Classes:
PascalCase

Methods:
PascalCase

Private Fields:
_camelCase

Interfaces:
IPrefix

---

# API Standards

Versioning:

```text
/api/v1
```

Provider Routes:

```text
/api/v1/emr/epic
/api/v1/emr/cerner
/api/v1/emr/altera
/api/v1/emr/athena-flow
/api/v1/emr/athena-server
```

Swagger required.

---

# Database Standards

EF Core Fluent API only.

No Data Annotations.

All entities must include:

- Id
- CreatedAtUtc
- UpdatedAtUtc

---

# Scenario Engine

Required Scenarios:

- Happy Path
- Patient Not Found
- Invalid Credentials
- Unauthorized
- Timeout
- Server Error
- Rate Limited
- Malformed Response

Must be database driven.

---

# Admin UI Requirements

Dashboard

Patient Management

Scenario Management

Provider Configuration

Request Log Viewer

Import Wizard

---

# Import Requirements

Support:

- CSV
- JSON

Validation:

- Required fields
- Duplicate detection
- Import report

---

# Logging

Capture:

- Request Headers
- Request Body
- Response Body
- Response Code
- Duration

Persist to database.

---

# Testing Requirements

Minimum Coverage Target:

80%

Required:

- Unit Tests
- Integration Tests

Frameworks:

- xUnit
- FluentAssertions

---

# CI/CD

GitHub Actions

Pipeline:

1. Restore
2. Build
3. Test
4. Publish

Build must fail on test failures.

---

# Docker Requirements

Provide:

Dockerfile (API)

Dockerfile (UI)

docker-compose.yml

---

# Definition of Done

Feature is complete only if:

- Code compiles
- Tests pass
- Swagger updated
- Documentation updated
- No critical warnings
- Feature manually verified

---

# POC Milestones

Milestone 1
Foundation

Milestone 2
Database

Milestone 3
Provider Engine

Milestone 4
Scenario Engine

Milestone 5
Admin UI

Milestone 6
Testing

Milestone 7
Documentation

---

# Expected Deliverables

- Source Code
- Unit Tests
- Integration Tests
- Swagger
- Database Migrations
- Seed Data
- Docker Support
- README
- Architecture Diagram

---

# Agent Execution Instructions

1. Generate solution structure.
2. Generate domain models.
3. Generate EF Core layer.
4. Generate provider engine.
5. Generate scenario engine.
6. Generate APIs.
7. Generate Angular UI.
8. Generate tests.
9. Generate Docker assets.
10. Generate documentation.

Do not skip tests.

Prefer maintainability over cleverness.

Follow Clean Architecture at all times.
