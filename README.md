# EMR Simulator Developer Portal

A local, offline EMR simulator that emulates Epic, Cerner, Altera, Athena Flow, and Athena Server. It provides provider-specific routes, deterministic failure scenarios, synthetic clinical data management, request logging, and a Swagger-documented REST API — all without requiring a live EMR connection.

## Overview

| Layer | Technology |
|---|---|
| API | .NET 8 / ASP.NET Core minimal API (`/api/v1`) |
| Admin UI | Angular 20+ standalone app |
| Testing | xUnit + FluentAssertions |
| Storage | SQLite via EF Core, with in-memory simulator support |

## Project Structure

```text
src/
├── EmrSimulator.Api/           # ASP.NET Core host, route handlers, Swagger
├── EmrSimulator.Application/   # Interfaces, scenario engine, import logic
├── EmrSimulator.Contracts/     # Shared request/response DTOs
├── EmrSimulator.Domain/        # Entities: providers, scenarios, patients, etc.
├── EmrSimulator.Infrastructure/# In-memory store, facade, DI extensions
└── EmrSimulator.AdminUi/       # Angular admin portal

tests/
├── EmrSimulator.Tests.Unit/        # Scenario engine unit tests
├── EmrSimulator.Tests.Contracts/   # Provider and clinical route contract tests
└── EmrSimulator.Tests.Integration/ # Provider switching, failure, import tests
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/) and npm

## Getting Started

### Backend API

```powershell
dotnet run --project src/EmrSimulator.Api/EmrSimulator.Api.csproj
```

Swagger UI is available at `http://localhost:5288/swagger` when using the default development launch profile.

### Admin UI

```powershell
cd src/EmrSimulator.AdminUi
npm install --legacy-peer-deps
npm start
```

Run Admin UI commands from `src/EmrSimulator.AdminUi`, where `angular.json` lives. The dev server starts at `http://localhost:4200` and proxies `/api` calls to the local API at `http://localhost:5288`.
Angular CLI cache files under `src/EmrSimulator.AdminUi/.angular/` are local artifacts and are ignored by Git.

## Key Capabilities

- **Provider switching** — switch between Epic, Cerner, Altera, Athena Flow, and Athena Server via the admin UI or API.
- **Deterministic scenarios** — select failure modes (not found, timeout, server error, rate limited, unauthorized, malformed response) that reproduce identically on every request.
- **Synthetic data management** — view, seed, and manage patients, appointments, orders, and results through the admin UI.
- **CSV / JSON import** — upload patient data with duplicate detection and per-row validation reporting.
- **Request logs** — every simulated request is logged with provider, scenario, headers, body, response, and duration.

## Running Tests

```powershell
# All tests
dotnet test

# Specific project
dotnet test tests/EmrSimulator.Tests.Unit/EmrSimulator.Tests.Unit.csproj
dotnet test tests/EmrSimulator.Tests.Contracts/EmrSimulator.Tests.Contracts.csproj
dotnet test tests/EmrSimulator.Tests.Integration/EmrSimulator.Tests.Integration.csproj
```

## Design Constraints

- Synthetic data only — no PHI is stored or transmitted.
- Offline by default — no external network dependencies at runtime.
- All routes versioned under `/api/v1`.
- Clean Architecture: `Api` → `Application` ← `Infrastructure`, `Domain` shared by all layers.
- Scenario behavior is deterministic and reproducible across identical requests.

## Specs

Current Iteration 3 feature specification and implementation plan are in [`specs/003-iteration-3/`](specs/003-iteration-3/).
