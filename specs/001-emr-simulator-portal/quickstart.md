# Quickstart: EMR Simulator Developer Portal

## Prerequisites

- .NET 8 SDK
- Node.js 20+ for the Angular admin UI
- SQLite-compatible local environment

## Run the simulator locally

1. Restore and build the solution.
2. Start the API host.
3. Start the Angular admin UI.
4. Open Swagger to inspect the versioned simulator routes.
5. Seed synthetic provider data and select a provider profile.

## Validate core workflows

1. Switch between Epic, Cerner, Altera, Athena Flow, and Athena Server.
2. Execute a representative patient lookup or workflow for each provider.
3. Toggle a scenario such as patient not found, timeout, or server error.
4. Verify request logs capture request headers, body, response, and duration.
5. Import a CSV or JSON patient file and confirm duplicate detection and validation reporting.

## Expected outcomes

- The simulator runs without a live external EMR dependency.
- Responses are deterministic for the active scenario and provider.
- Swagger documents the exposed `/api/v1` routes.
- Request history and import results are visible in the admin UI.