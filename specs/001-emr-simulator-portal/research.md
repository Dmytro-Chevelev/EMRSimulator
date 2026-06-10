# Research: EMR Simulator Developer Portal

## Decision 1: Use .NET 8 with ASP.NET Core Web API for the simulator backend

- Decision: Implement the API and simulator runtime on .NET 8.
- Rationale: The architecture documents and ADR explicitly select .NET 8, and it aligns with the requested provider routing, Swagger, and testing stack.
- Alternatives considered: A different backend runtime was not pursued because it would conflict with the documented simulator stack.

## Decision 2: Use a Clean Architecture layout with explicit layer boundaries

- Decision: Separate the solution into Api, Application, Domain, Infrastructure, AdminUi, and Contracts projects.
- Rationale: The design documents require strong dependency direction and isolated provider, persistence, and UI concerns.
- Alternatives considered: A flatter project structure was rejected because it would blur boundaries and make provider-specific behavior harder to contain.

## Decision 3: Use SQLite and EF Core Fluent API for persistence

- Decision: Persist simulator state, scenarios, and logs in SQLite using EF Core mappings configured with Fluent API.
- Rationale: The source documents call for a local, zero-infrastructure store and explicitly require Fluent API rather than data annotations.
- Alternatives considered: In-memory-only storage was rejected because it would not support request logs, import persistence, or reproducible scenario state.

## Decision 4: Model provider behavior as deterministic scenarios

- Decision: Drive all provider responses through scenario state and seeded data so repeated requests produce the same outcome.
- Rationale: Determinism is required for debugging, reproducible QA, and the documented failure cases.
- Alternatives considered: Randomized mock responses were rejected because they would break reproducibility and make CI unreliable.

## Decision 5: Expose provider routes and admin workflows under a versioned API with Swagger

- Decision: Publish simulator endpoints under `/api/v1` and keep Swagger available for exploration and validation.
- Rationale: The architecture and endpoint inventory document a versioned surface and the constitution requires Swagger updates with behavior changes.
- Alternatives considered: Unversioned routes were rejected because the documented contract inventory already assumes stable versioned paths.

## Decision 6: Implement the admin experience as an Angular 20+ SPA

- Decision: Use Angular 20+ with Angular Material for the admin portal.
- Rationale: The ADR and agent build specification prefer Angular for the UI and the admin workflows include dashboard, patient management, logs, and import flows.
- Alternatives considered: A Blazor UI was not selected because the accepted design direction already favors Angular.