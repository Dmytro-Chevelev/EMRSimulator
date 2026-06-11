# Quickstart: EMR Simulator Developer Portal

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/) and npm 9+

## Run the API locally

```powershell
cd src/EmrSimulator.Api
dotnet run
```

Swagger UI is available at `http://localhost:<port>/swagger`.

## Run the Admin UI locally

> **Important**: Run all Angular commands from `src/EmrSimulator.AdminUi/` (where `angular.json` lives).

```powershell
cd src/EmrSimulator.AdminUi
npm install
npm start        # equivalent to ng serve
```

The admin portal opens at `http://localhost:4200`.

## Run tests

```powershell
# All tests
dotnet test

# Individual projects
dotnet test tests/EmrSimulator.Tests.Unit/EmrSimulator.Tests.Unit.csproj
dotnet test tests/EmrSimulator.Tests.Contracts/EmrSimulator.Tests.Contracts.csproj
dotnet test tests/EmrSimulator.Tests.Integration/EmrSimulator.Tests.Integration.csproj
```

## Build the Admin UI (production)

```powershell
cd src/EmrSimulator.AdminUi
npm run build
# Output lands in src/EmrSimulator.AdminUi/dist/emr-simulator-admin-ui/
```

## Validate core workflows

1. Switch between Epic, Cerner, Altera, Athena Flow, and Athena Server via the Providers page.
2. Execute a representative patient lookup or workflow for each provider using Swagger.
3. Toggle a scenario such as patient not found, timeout, or server error via the Scenarios page.
4. Verify request logs capture request headers, body, response, and duration via the Request Logs page.
5. Import a CSV or JSON patient file via the Imports page and confirm duplicate detection and validation reporting.

## Expected outcomes

- The simulator runs without a live external EMR dependency.
- Responses are deterministic for the active scenario and provider.
- Swagger documents the exposed `/api/v1` routes.
- Request history and import results are visible in the admin UI.
- SQLite database (`emrsimulator.db`) persists state across process restarts (Iteration 2+).
